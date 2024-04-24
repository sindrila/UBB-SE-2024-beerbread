namespace app.MVVM.Model.Data.SqlCommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Text;
    using app.MVVM.Model.Data.ServerHandlers;
    using app.MVVM.Model.Domain;
    using Microsoft.Data.SqlClient;

    public interface ISqlSongTableCommandExecutor
    {
        List<int> GetAllSongIdsInDatabase();

        List<Song> GetSongsWithIds(List<int> songIds);
    }

    internal class SqlSongTableCommandExecutor : SqlCommandExecutor, ISqlSongTableCommandExecutor
    {
        private readonly SqlConnection currentSqlConnection;

        public SqlSongTableCommandExecutor()
        {
            currentSqlConnection = StaticSqlConnectionGenerator.GetConnection();
        }

        public List<int> GetAllSongIdsInDatabase()
        {

            List<int> allSongIds = new List<int>();
            try
            {
                currentSqlConnection.Open();
                SqlCommand command = new("SELECT id FROM songs", currentSqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    allSongIds.Add(reader.GetInt32("id"));
                }

                reader.Close();
                currentSqlConnection.Close();
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
            }

            return allSongIds;
        }

        public List<Song> GetSongsWithIds(List<int> songIds)
        {
            List<Song> songs = new List<Song>();
            try
            {
                currentSqlConnection.Open();
                string stringIds = string.Empty;
                foreach (int id in songIds)
                {
                    stringIds += id + ",";
                }

                SqlCommand command = new("SELECT * FROM Songs WHERE id in (" + stringIds.Substring(0, stringIds.Length - 1) + ")", currentSqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    songs.Add(new Song(
                               reader.GetInt32("id"),
                               reader.GetString("songName"),
                               reader.GetString("artistName"),
                               reader.GetString("urlSong"),
                               reader.GetString("urlImage")));
                }

                reader.Close();
                currentSqlConnection.Close();
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
            }

            return songs;
        }
    }
}
