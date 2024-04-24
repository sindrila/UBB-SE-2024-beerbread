using Xunit;
using app.MVVM.Model.Domain;

namespace MusicAppTests
{
    public class AccountTests
    {
        [Fact]
        public void GetHashedPassword_ReturnsHashedPassword()
        {
            // Arrange
            string email = "test@example.com";
            string username = "user";
            string salt = "randomSalt";
            string hashedPassword = "hashedPassword";

            var account = new Account(email, username, salt, hashedPassword);

            // Act
            string result = account.GetHashedPassword();

            // Assert
            Assert.Equal(hashedPassword, result);
        }

        [Fact]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            string email = "test@example.com";
            string username = "user";
            string salt = "randomSalt";
            string password = "password123";
            string hashedPassword = "hashedPassword"; // In real scenario, this should be hashed

            var account = new Account(email, username, salt, hashedPassword);

            // Act
            bool result = account.VerifyPassword(hashedPassword); // In real scenario, this should be hashed

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            string email = "test@example.com";
            string username = "user";
            string salt = "randomSalt";
            string password = "password123";
            string hashedPassword = "hashedPassword"; // In real scenario, this should be hashed

            var account = new Account(email, username, salt, hashedPassword);

            // Act
            bool result = account.VerifyPassword("incorrectPassword"); // In real scenario, this should be hashed

            // Assert
            Assert.False(result);
        }
    }
}
