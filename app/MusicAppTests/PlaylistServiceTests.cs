namespace MusicAppTests
{
    using Moq;
    using app.MVVM.ViewModel;
    using app.MVVM.Model.Domain;
    using app.MVVM.Model.Data.Repositories;
    public class PlaylistServiceTests
    {
        [Fact]
        public void PlaylistServiceTests_EmptyConstructor()
        {
            var playlistService = new PlaylistService();
            Assert.IsType<PlaylistService>(playlistService);
        }

        [Fact]
        public void GetUserPlaylists_ReturnsUserPlaylists()
        {
            // Arrange
            var mockRepository = new Mock<ISqlPlaylistRepository>();
            Guid userId = Guid.NewGuid();
            List<IPlaylist> expectedPlaylists =
            [
                new Playlist(1, 1, "Playlist 1"),
                new Playlist(2, 1, "Playlist 2"),
                new Playlist(3, 1, "Playlist 3")
            ];
            mockRepository.Setup(repo => repo.GetUserPlaylists(userId)).Returns(expectedPlaylists);
            var playlistService = new PlaylistService(mockRepository.Object);

            // Act
            List<IPlaylist> result = playlistService.GetUserPlaylists(userId);

            // Assert
            Assert.Equal(expectedPlaylists, result);
        }

        [Fact]
        public void AddPlaylist_CallsRepositoryMethod()
        {
            // Arrange
            var mockRepository = new Mock<ISqlPlaylistRepository>();
            var playlistService = new PlaylistService(mockRepository.Object);
            IPlaylist playlist = new Playlist(1, 1, "New Playlist");
            Guid userId = Guid.NewGuid();

            // Act
            playlistService.AddPlaylist(playlist, userId);

            // Assert
            mockRepository.Verify(repo => repo.AddPlaylist(playlist, userId), Times.Once);
        }
    }
}
