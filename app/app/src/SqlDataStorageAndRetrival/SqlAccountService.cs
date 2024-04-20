using app.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.src.SqlDataStorageAndRetrival
{
    public class SqlAccountService :ISqlAccountService
    {
        private List<Account> accounts = [];
        Microsoft.Data.SqlClient.SqlConnection sqlconnection;
        public SqlAccountService()
        {
            // Connect to database
            sqlconnection = SqlConnection.GetConnection();
        }
        public bool AddAccount(Account account)
        {
            // insert account into database
            string insertQuery = "INSERT INTO accounts (guid, email, username, salt, hashedPassword) " +
                "VALUES ('" + account.Id + "', '" + account.Email + "', '" + account.Username + "', '" + account.Salt + "', '" + account.GetHashedPassword() + "')";
            try
            {
                sqlconnection.Open();
                Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(insertQuery, sqlconnection);
                command.ExecuteNonQuery();
                sqlconnection.Close();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        public bool AddUserAccount(Account account)
        {
            sqlconnection.Open();
            string get_AccountIdGuid_Query = "SELECT id,guid FROM accounts WHERE email = '" + account.Email + "'";
            Microsoft.Data.SqlClient.SqlCommand command = new(get_AccountIdGuid_Query, sqlconnection);
            Microsoft.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int id = reader.GetInt32(0);
            string guid = reader.GetGuid(1).ToString();
            reader.Close();

            string create_HistoryPlaylist_Query = "INSERT INTO playlists (name, isPrivate, owner) " +
                "VALUES ('" + guid + "_History', 1, " + id + ")";
            string create_LikedPlaylist_Query = "INSERT INTO playlists (name, isPrivate, owner) " +
                "VALUES ('" + guid + "_Liked', 1, " + id + ")";
            string create_BlockedPlaylist_Query = "INSERT INTO playlists (name, isPrivate, owner) " +
                "VALUES ('" + guid + "_Blocked', 1, " + id + ")";
            try
            {

                Microsoft.Data.SqlClient.SqlCommand command1 = new(create_HistoryPlaylist_Query, sqlconnection);
                command1.ExecuteNonQuery();
                Microsoft.Data.SqlClient.SqlCommand command2 = new(create_LikedPlaylist_Query, sqlconnection);
                command2.ExecuteNonQuery();
                Microsoft.Data.SqlClient.SqlCommand command3 = new(create_BlockedPlaylist_Query, sqlconnection);
                command3.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                sqlconnection.Close();
                return false;
            }

            string create_UserAccount_Query = "INSERT INTO users (id, historyPlaylist, likedPlaylist, blockedplaylist) " +
                "VALUES (" + id + ", " +
                "(SELECT id FROM playlists WHERE name = '" + guid + "_History'), " +
                "(SELECT id FROM playlists WHERE name = '" + guid + "_Liked'), " +
                "(SELECT id FROM playlists WHERE name = '" + guid + "_Blocked'))";

            try
            {
                Microsoft.Data.SqlClient.SqlCommand command4 = new(create_UserAccount_Query, sqlconnection);
                command4.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                sqlconnection.Close();
                return false;
            }

            sqlconnection.Close();
            return true;
        }

        public Account GetAccount(string email)
        {
            // Get account from database
            string selectQuery = "SELECT guid, username, salt, hashedPassword FROM accounts WHERE email = '" + email + "'";
            try
            {
                sqlconnection.Open();
                Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(selectQuery, sqlconnection);
                Microsoft.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {

                    Account temp = new Account(reader.GetGuid("guid"),email, reader.GetString("username"), reader.GetString("salt"), reader.GetString("hashedPassword"));
                    sqlconnection.Close();
                    return temp;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                sqlconnection.Close();
            }
            return null;
        }

        public String GetAccountHashedPassword(string email)
        {
            // Get account from database
            string selectQuery = "SELECT hashedPassword FROM accounts WHERE email = '" + email + "'";
            try
            {
                sqlconnection.Open();
                Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(selectQuery, sqlconnection);
                Microsoft.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    sqlconnection.Close();
                    return reader.GetString(0);
                }

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                sqlconnection.Close();
            }
            return null;
        }
    }

}
