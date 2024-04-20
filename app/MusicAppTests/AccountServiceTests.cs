using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using app;
using app.src.SqlDataStorageAndRetrival;
using Moq; // for mocking dependencies

namespace MusicAppTests
{
    public class AccountServiceTests
    {
        private static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        // Test to check if a new user account is created successfully


        // Test to check if creating a user account with existing email fails


        // Test to check user authentication


        // Test for invalid email format
        [Fact]
        public void TestIsValidEmail_Invalid()
        {
            // Arrange
            var accountService = new AccountService();

            // Act
            var result = accountService.IsValidEmail("invalid-email");

            // Assert
            Assert.False(result);
        }

        // Test for valid email format
        [Fact]
        public void TestIsValidEmail_Valid()
        {
            // Arrange
            var accountService = new AccountService();

            // Act
            var result = accountService.IsValidEmail("valid@example.com");

            // Assert
            Assert.True(result);
        }

        // Test to check password hashing
        [Fact]
        public void TestCreateArtistAccount()
        {
            var artistService=new AccountService();
            var result=artistService.CreateArtistAccount("","","");
            Assert.False(result);
        }

        [Fact]
        public void TestGenerateSalt_ShouldReturnBase64StringOfExpectedLength()
        {

            var accountService = new AccountService();
            // Arrange
            var expectedLength = 44; // A 32-byte array encoded to Base64 has 44 characters

            // Assuming GenerateSalt is a static method in a class named AccountService
            var generatedSalt = accountService.GenerateSalt();

            // Act & Assert
            Assert.Equal(expectedLength, generatedSalt.Length);
        }

        [Fact]
        public void HashPassword_ShouldReturnDeterministicOutput()
        {
            // Arrange
            var password = "MySecurePassword";
            var salt = "SomeRandomSalt";

            // Act
            var hash1 = HashPassword(password, salt);
            var hash2 = HashPassword(password, salt);

            // Assert
            Assert.Equal(hash1, hash2); // Both hashes should be identical
        }
        [Fact]
        public void HashPassword_ShouldReturnValidBase64String()
        {
            // Arrange
            var password = "MySecurePassword";
            var salt = "SomeRandomSalt";

            // Act
            var hashedPassword = HashPassword(password, salt);

            // Validate if the generated hash is a valid Base64 string
            var isValidBase64 = IsValidBase64(hashedPassword);

            // Assert
            Assert.True(isValidBase64);
        }

        [Fact]
        public void HashPassword_ShouldReturnExpectedLength()
        {
            // Arrange
            var password = "MySecurePassword";
            var salt = "SomeRandomSalt";
            var expectedLength = 44; // Base64 length for 32-byte SHA256 hash

            // Act
            var hashedPassword = HashPassword(password, salt);

            // Assert
            Assert.Equal(expectedLength, hashedPassword.Length);
        }

        private bool IsValidBase64(string base64)
        {
            try
            {
                Convert.FromBase64String(base64);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }

}
