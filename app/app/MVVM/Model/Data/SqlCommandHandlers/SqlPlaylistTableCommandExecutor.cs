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

    public interface ISqlPlaylistTableCommandExecutor
    {
        List<IPlaylist> GetUserPlaylistIdsWithGuid(Guid userGuid);

        string GetImagePathForTopSongInPlaylistWithId(int playlistId);

        bool ExecuteInsertPlaylistNonQueryCommand(string userGuid, string playlistName, int isPrivate);
    }

    internal class SqlPlaylistTableCommandExecutor : SqlCommandExecutor, ISqlPlaylistTableCommandExecutor
    {
        private SqlConnection currentSqlConnection;

        public SqlPlaylistTableCommandExecutor()
        {
            currentSqlConnection = StaticSqlConnectionGenerator.GetConnection();
        }

        public List<IPlaylist> GetUserPlaylistIdsWithGuid(Guid userGuid)
        {
            List<IPlaylist> userPlaylists = [];
            try
            {
                currentSqlConnection.Open();
                SqlCommand command = new ("SELECT * FROM Playlists where owner = (select id from accounts where guid='" + userGuid.ToString() + "')", currentSqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userPlaylists.Add(new Playlist(
                                    reader.GetInt32("id"),
                                    reader.GetInt32("owner"),
                                    reader.GetString("name"),
                                    reader.GetBoolean("isPrivate")));
                }

                reader.Close();
                currentSqlConnection.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }

            return userPlaylists;
        }

        public string GetImagePathForTopSongInPlaylistWithId(int playlistId)
        {
            string imagePath = string.Empty;
            try
            {
                currentSqlConnection.Open();
                SqlCommand command = new("Select urlImage from songs where id = (SELECT TOP 1 idSong FROM songs_in_playlist Where idPlaylist = " + playlistId + ")", currentSqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                imagePath = reader.GetString("urlImage");
                reader.Close();
                currentSqlConnection.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }

            return imagePath;
        }

        public bool ExecuteInsertPlaylistNonQueryCommand(string userGuid, string playlistName, int isPrivate)
        {
            string command = "INSERT INTO Playlists (owner, name, isPrivate) " +
                             "VALUES ((select id from accounts where guid='" +
                             userGuid + "'), '" + playlistName + "', " + isPrivate + ")";
            return ExecuteNonQueryCommandFromString(command);
        }

    }
}
