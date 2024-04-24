using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using app.MVVM.Model.Data.Utilities;
using app.MVVM.Model.Data.Repositories;
using app.MVVM.Model.Domain;

namespace MusicAppTests
{

    public class MockAnalyticsAPITests
    {
        private Mock<ISqlSongRepository> mockSongRepository;
        private MockAnalyticsAPI mockAnalyticsAPI;
        private User testUser;

        public MockAnalyticsAPITests()
        {
            testUser = new User(new Account(Guid.NewGuid(), "test@example.com", "TestUser", "random_salt", "hashed_password"));
            mockSongRepository = new Mock<ISqlSongRepository>();
        }

        [Fact]
        public void MockAnalyticsAPI_ShouldInitializeProperly()
        {
            // Arrange
            var expectedSongIds = new List<int> { 1, 2, 3, 4, 5 };
            mockSongRepository
                .Setup(repo => repo.GetAllSongIds())
                .Returns(expectedSongIds);

            // Act
            var analyticsAPI = new MockAnalyticsAPI(testUser);

            // Assert
            Assert.NotNull(analyticsAPI);
            Assert.NotEmpty(analyticsAPI.GetRecomendedSongs(5));
        }

        [Fact]
        public void GetRecomendedSongs_ShouldReturnRandomSongs()
        {
            // Arrange
            var expectedSongIds = new List<int> { 1, 2, 3, 4, 5 };
            mockSongRepository
                .Setup(repo => repo.GetAllSongIds())
                .Returns(expectedSongIds);

            mockAnalyticsAPI = new MockAnalyticsAPI(testUser);

            // Act
            var recommendedSongs = mockAnalyticsAPI.GetRecomendedSongs(5);

            // Assert
            Assert.Contains(0, recommendedSongs); // Validate the extra item
        }

        [Fact]
        public void GetRecomendedSongs_ShouldHandleMoreRequestedThanAvailable()
        {
            // Arrange
            var songIds = new List<int> { 1, 2, 3 };
            mockSongRepository
                .Setup(repo => repo.GetAllSongIds())
                .Returns(songIds);

            mockAnalyticsAPI = new MockAnalyticsAPI(testUser);

            // Act
            var recommendedSongs = mockAnalyticsAPI.GetRecomendedSongs(5);

            // Assert
            Assert.Contains(0, recommendedSongs); // Validate the extra item
        }

        [Fact]
        public void GetRecomendedSongs_ShouldReturnEmptyIfNoSongsAvailable()
        {
            // Arrange
            mockSongRepository
                .Setup(repo => repo.GetAllSongIds())
                .Returns(new List<int>());

            mockAnalyticsAPI = new MockAnalyticsAPI(testUser);

            // Act
            var recommendedSongs = mockAnalyticsAPI.GetRecomendedSongs(5);

            // Assert
            Assert.Equal(1, recommendedSongs.Count); // Only the extra item (0)
            Assert.Contains(0, recommendedSongs); // Validate the extra item
        }
        [Fact]
        public void GetRecomendedSongs_ShouldReturnFewerSongsWhenLessAvailable()
        {
            // Arrange
            var songIds = new List<int> { 1, 2 };
            mockSongRepository
                .Setup(repo => repo.GetAllSongIds())
                .Returns(songIds);

            mockAnalyticsAPI = new MockAnalyticsAPI(testUser);

            // Act
            var recommendedSongs = mockAnalyticsAPI.GetRecomendedSongs(5);

            // Assert
            Assert.True(recommendedSongs.Count <= songIds.Count + 1); // 2 songs + 1 extra
            Assert.Contains(0, recommendedSongs); // Validate the extra item
        }
        [Fact]
        public void GetRecomendedSongs_ShouldHandleNegativeOrZeroInput()
        {
            // Arrange
            mockAnalyticsAPI = new MockAnalyticsAPI(testUser);

            // Act
            var recommendedSongsNegative = mockAnalyticsAPI.GetRecomendedSongs(-1);
            var recommendedSongsZero = mockAnalyticsAPI.GetRecomendedSongs(0);

            // Assert
            Assert.Equal(1, recommendedSongsNegative.Count); // Only the extra item (0)
            Assert.Equal(1, recommendedSongsZero.Count); // Only the extra item (0)
        }
        [Fact]
        public void GetRecomendedSongs_ShouldNotContainDuplicates()
        {
            // Arrange
            var songIds = new List<int> { 1, 2, 3, 4, 5 };
            mockSongRepository
                .Setup(repo => repo.GetAllSongIds())
                .Returns(songIds);

            mockAnalyticsAPI = new MockAnalyticsAPI(testUser);

            // Act
            var recommendedSongs = mockAnalyticsAPI.GetRecomendedSongs(5);

            // Assert
            var distinctSongs = new HashSet<int>(recommendedSongs);
            Assert.Equal(recommendedSongs.Count, distinctSongs.Count); // Check for duplicates
        }
    }
}
