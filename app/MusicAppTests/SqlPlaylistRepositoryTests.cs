using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using app.MVVM.Model.Data.SqlCommandHandlers;
using app.MVVM.Model.Domain;
using app.MVVM.Model.Data.Repositories;

namespace MusicAppTests
{
    public class SqlPlaylistRepositoryTests
    {
        [Fact]
        public void SqlPlaylistRepository_EmptyConstructor()
        {
            var playlistRepository = new SqlPlaylistRepository();
            Assert.IsType<SqlPlaylistRepository>(playlistRepository);
        }
        [Fact]
        public void GetUserPlaylists_ValidUserGuid_ReturnsUserPlaylists()
        {
            // Arrange
            Guid userGuid = Guid.NewGuid();
            var mockCommandExecutor = new Mock<ISqlPlaylistTableCommandExecutor>();
            var expectedPlaylists = new List<IPlaylist>
            {
                new Playlist(1, 1, "Playlist 1", false, "path1"),
                new Playlist(2, 1, "Playlist 2", true, "path2")
            };
            mockCommandExecutor.Setup(e => e.GetUserPlaylistIdsWithGuid(userGuid)).Returns(expectedPlaylists);
            var playlistRepository = new SqlPlaylistRepository(mockCommandExecutor.Object);

            // Act
            List<IPlaylist> result = playlistRepository.GetUserPlaylists(userGuid);

            // Assert
            Assert.Equal(expectedPlaylists.Count, result.Count);
            for (int i = 0; i < expectedPlaylists.Count; i++)
            {
                Assert.Equal(expectedPlaylists[i].Id, result[i].Id);
                Assert.Equal(expectedPlaylists[i].Name, result[i].Name);
                Assert.Equal(expectedPlaylists[i].IsPrivate, result[i].IsPrivate);
                Assert.Equal(expectedPlaylists[i].ImagePath, result[i].ImagePath);
            }
        }

        [Fact]
        public void AddPlaylist_ValidPlaylist_ReturnsTrue()
        {
            // Arrange
            Guid userGuid = Guid.NewGuid();
            var mockCommandExecutor = new Mock<ISqlPlaylistTableCommandExecutor>();
            mockCommandExecutor.Setup(e => e.ExecuteInsertPlaylistNonQueryCommand(userGuid.ToString(), "Test Playlist", 0)).Returns(true);
            var playlistRepository = new SqlPlaylistRepository(mockCommandExecutor.Object);
            var playlist = new Playlist(1, 1, "Test Playlist");

            // Act
            bool result = playlistRepository.AddPlaylist(playlist, userGuid);

            // Assert
            Assert.True(result);
        }
    }
}
