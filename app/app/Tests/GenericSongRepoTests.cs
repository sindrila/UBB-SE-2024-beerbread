using Xunit;
using app.Interfaces;

namespace app.Tests
{
    public class GenericSongRepoTests
    {
        [Fact]
        public void AddSong_ValidSongId_ReturnsTrue()
        {
            // Arrange
            var repo = new GenericSongRepo(1, 1, "TestRepo");

            // Act
            bool result = repo.AddSong(123);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AddSong_DuplicateSongId_ReturnsFalse()
        {
            // Arrange
            var repo = new GenericSongRepo(1, 1, "TestRepo");
            repo.AddSong(123);

            // Act
            bool result = repo.AddSong(123);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveSong_ExistingSongId_ReturnsTrue()
        {
            // Arrange
            var repo = new GenericSongRepo(1, 1, "TestRepo");
            repo.AddSong(123);

            // Act
            bool result = repo.RemoveSong(123);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void RemoveSong_NonExistingSongId_ReturnsFalse()
        {
            // Arrange
            var repo = new GenericSongRepo(1, 1, "TestRepo");

            // Act
            bool result = repo.RemoveSong(123);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetSongsNumber_EmptyRepo_ReturnsZero()
        {
            // Arrange
            var repo = new GenericSongRepo(1, 1, "TestRepo");

            // Act
            int count = repo.GetSongsNumber();

            // Assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void GetSongsNumber_RepoWithSongs_ReturnsCorrectCount()
        {
            // Arrange
            var repo = new GenericSongRepo(1, 1, "TestRepo");
            repo.AddSong(123);
            repo.AddSong(456);
            repo.AddSong(789);

            // Act
            int count = repo.GetSongsNumber();

            // Assert
            Assert.Equal(3, count);
        }

        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            int ownerId = 1;
            int id = 1;
            string name = "TestRepo";

            // Act
            var repo = new GenericSongRepo(ownerId, id, name);

            // Assert
            Assert.Equal(ownerId, repo.owner);
            Assert.Equal(id, repo.id);
            Assert.Equal(name, repo.name);
            Assert.NotNull(repo.songs);
            Assert.Empty(repo.songs);
        }
    }
}
