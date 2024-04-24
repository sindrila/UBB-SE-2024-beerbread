namespace app.MVVM.Model.Data.SqlCommandHandlers
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Text;
    using app.MVVM.Model.Data.ServerHandlers;
    using app.MVVM.Model.Domain;
    using Microsoft.Data.SqlClient;

    public interface ISqlUserTableCommandExecutor
    {
        bool ExecuteNonQueryCommandFromString(string query);

        IPlaylist? GetHistoryPlaylistForUserWithId(int userId);

        IPlaylist? GetLikedPlaylistForUserWithId(int userId);

        IPlaylist? GetBlockedPlaylistForUserWithId(int userId);

        List<IPlaylist> GetUserDefinedPlaylistsForUserWithId(int userId, int historyPlaylistId, int likedPlaylistId, int blockedPlaylistId);
    }

    internal class SqlUserTableCommandExecutor : SqlCommandExecutor, ISqlUserTableCommandExecutor
    {
        private readonly SqlConnection currentSqlConnection;

        public SqlUserTableCommandExecutor()
        {
            currentSqlConnection = StaticSqlConnectionGenerator.GetConnection();
        }

        public IPlaylist? GetBlockedPlaylistForUserWithId(int userId)
        {
            try
            {
                currentSqlConnection.Open();
                SqlCommand command = new("select * from playlists where id = (select likedPlaylist from users where id=" + userId + ")", currentSqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                IPlaylist likedPlaylist = new Playlist(
                    int.Parse(reader.GetString("id")),
                    int.Parse(reader.GetString("owner")),
                    reader.GetString("name"),
                    reader.GetBoolean("isPrivate"));
                reader.Close();
                currentSqlConnection.Close();
                return likedPlaylist;
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                return null;
            }
        }

        public IPlaylist? GetHistoryPlaylistForUserWithId(int userId)
        {

            try
            {
                currentSqlConnection.Open();
                SqlCommand command = new("select * from playlists where id = (select historyPlaylist from users where id=" + userId + ")", currentSqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                IPlaylist historyPlaylist = new Playlist(
                    int.Parse(reader.GetString("id")),
                    int.Parse(reader.GetString("owner")),
                    reader.GetString("name"),
                    reader.GetBoolean("isPrivate"));
                reader.Close();
                currentSqlConnection.Close();
                return historyPlaylist;
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                return null;
            }
        }

        public IPlaylist? GetLikedPlaylistForUserWithId(int userId)
        {
            try
            {
                currentSqlConnection.Open();
                SqlCommand command = new("select * from playlists where id = (select blockedplaylist from users where id=" + userId + ")", currentSqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                IPlaylist blockedPlaylist = new Playlist(
                    int.Parse(reader.GetString("id")),
                    int.Parse(reader.GetString("owner")),
                    reader.GetString("name"),
                    reader.GetBoolean("isPrivate"));
                reader.Close();
                currentSqlConnection.Close();
                return blockedPlaylist;
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                return null;
            }
        }

        public List<IPlaylist> GetUserDefinedPlaylistsForUserWithId(int userId, int historyPlaylistId, int likedPlaylistId, int blockedPlaylistId)
        {
            List<IPlaylist> userDefinedPlaylists = [];
            try
            {
                currentSqlConnection.Open();
                SqlCommand command = new("select * from playlists where owner=" + userId + " and id not in(" + historyPlaylistId + "," + likedPlaylistId + "," + blockedPlaylistId + ")", currentSqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userDefinedPlaylists.Add(new Playlist(
                        int.Parse(reader.GetString("id")),
                        int.Parse(reader.GetString("owner")),
                        reader.GetString("name"),
                        reader.GetBoolean("isPrivate")));
                }

                reader.Close();
                currentSqlConnection.Close();
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
            }

            return userDefinedPlaylists;
        }
    }
}
