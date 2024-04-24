using Xunit;
using app.MVVM.Model.Domain;
using System;

namespace MusicAppTests
{
    public class UserTests
    {
        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string email = "test@example.com";
            string username = "user";
            string salt = "randomSalt";
            string hashPassword = "hashedPassword";

            var account = new Account(id, email, username, salt, hashPassword);

            // Act
            var user = new User(account);

            // Assert
            Assert.Equal(id, user.Id);
            Assert.Equal(email, user.Email);
            Assert.Equal(username, user.Username);
            Assert.Equal(salt, user.Salt);
            Assert.Equal(hashPassword, user.GetHashedPassword());
            Assert.Null(user.History);
            Assert.Null(user.LikedSongs);
            Assert.Null(user.BlockedSongs);
            Assert.NotNull(user.Playlists);
            Assert.Empty(user.Playlists); 
        }

    }
}
