using Xunit;
using Moq;
using app.MVVM.ViewModel;
using app.MVVM.Model.Domain;
using System.Collections.Generic;
using app.MVVM.Model.Data.SqlCommandHandlers;
using app.MVVM.Model.Data.Repositories;

namespace MusicAppTests
{
    public class SongServiceTests
    {
        [Fact]
        public void SongServiceTests_EmptyConstructor()
        {
            var songService = new SongServiceTests();
            Assert.IsType<SongServiceTests>(songService);
        }

        [Fact]
        public void GetSongsWithIds_ReturnsCorrectSongs()
        {
            // Arrange
            var mockCommandExecutor = new Mock<ISqlSongTableCommandExecutor>();
            var mockRepository = new Mock<ISqlSongRepository>();
            List<int> ids = new List<int> { 1, 2, 3 };
            List<Song> expectedSongs = new List<Song>
            {
                new Song(1, "Song 1", "Artist 1", "url1"),
                new Song(2, "Song 2", "Artist 2", "url2"),
                new Song(3, "Song 3", "Artist 3", "url3")
            };
            mockCommandExecutor.Setup(executor => executor.GetSongsWithIds(ids)).Returns(expectedSongs);
            mockRepository.Setup(repo => repo.GetSongsWithIds(ids)).Returns(expectedSongs);
            var songService = new SongService(mockRepository.Object);

            // Act
            List<Song> result = songService.GetSongsWithIds(ids);

            // Assert
            Assert.Equal(expectedSongs, result);
        }
    }
}
