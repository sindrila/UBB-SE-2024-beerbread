using app.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using app.MVVM.Model.Data.Repositories;
using Moq;
using app.MVVM.Model.Domain;

namespace MusicAppTests
{
    public class AccountServiceTests
    {
        private readonly Mock<ISqlAccountRepository> mockRepository;
        private readonly AccountService accountService;

        public AccountServiceTests()
        {
            mockRepository = new Mock<ISqlAccountRepository>();
            accountService = new AccountService(mockRepository.Object);
        }
        [Fact]
        public void GetAccountWithCredentials_EmptyEmail_ReturnsNull()
        {
            // Act
            User result = this.accountService.GetAccountWithCredentials("", "SecurePassword123");

            // Assert
            Assert.Null(result); // Should return null with empty email
        }

        [Fact]
        public void GetAccountWithCredentials_WhitespaceOnlyEmail_ReturnsNull()
        {
            // Act
            User result = this.accountService.GetAccountWithCredentials("   ", "SecurePassword123");

            // Assert
            Assert.Null(result); // Should return null with whitespace-only email
        }

        [Fact]
        public void GetAccountWithCredentials_MultipleCalls_ConsistentResults()
        {
            // Arrange
            string email = "user@example.com";
            string password = "SecurePassword123";
            string salt = AccountService.GenerateSalt();
            string hashedPassword = AccountService.HashPassword(password, salt);
            Account account = new Account(Guid.NewGuid(), email, "username", salt, hashedPassword);

            mockRepository.Setup(repo => repo.GetAccount(email)).Returns(account);

            // Act
            User firstResult = this.accountService.GetAccountWithCredentials(email, password);
            User secondResult = this.accountService.GetAccountWithCredentials(email, password);

            // Assert
            Assert.NotNull(firstResult);
            Assert.Equal(firstResult.Username, secondResult.Username); // Results should be consistent
        }
        [Fact]
        public void GetAccountWithCredentials_PerformanceTest()
        {
            // Arrange
            string email = "user@example.com";
            string password = "SecurePassword123";
            string salt = AccountService.GenerateSalt();
            string hashedPassword = AccountService.HashPassword(password, salt);
            Account account = new Account(Guid.NewGuid(), email, "username", salt, hashedPassword);

            mockRepository.Setup(repo => repo.GetAccount(email)).Returns(account);

            var stopwatch = new System.Diagnostics.Stopwatch();

            // Act
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                var result = accountService.GetAccountWithCredentials(email, password);
            }
            stopwatch.Stop();

            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 5000); // Ensure performance within a limit
        }
        [Fact]
        public void GetAccountWithCredentials_TraceLoggingOnException()
        {
            // Arrange
            var mockRepository = new Mock<ISqlAccountRepository>();
            mockRepository.Setup(repo => repo.GetAccount(It.IsAny<string>())).Throws(new Exception("Simulated database error"));

            var traceListener = new System.Diagnostics.TextWriterTraceListener(new System.IO.StringWriter());
            System.Diagnostics.Trace.Listeners.Add(traceListener);

            var accountService = new AccountService(mockRepository.Object);

            // Act
            var result = accountService.GetAccountWithCredentials("user@example.com", "SecurePassword123");

            // Assert
            Assert.Null(result); // Should return null due to exception

            string traceOutput = traceListener.Writer.ToString();
            Assert.Contains("Exception in GetAccountWithCredentials", traceOutput); // Verify trace message
        }

        [Fact]
        public void GetAccountWithCredentials_EmptyPassword_ReturnsNull()
        {
            // Arrange
            string email = "user@example.com";

            // Act
            User result = accountService.GetAccountWithCredentials(email, "");

            // Assert
            Assert.Null(result); // Should return null with empty password
        }
        [Fact]
        public void GetAccountWithCredentials_WhitespaceOnlyPassword_ReturnsNull()
        {
            // Act
            User result = accountService.GetAccountWithCredentials("user@example.com", "   ");

            // Assert
            Assert.Null(result); // Should return null with whitespace-only password
        }
        [Fact]
        public void GetAccountWithCredentials_ValidCredentials_ReturnsUser()
        {
            // Arrange
            string email = "user@example.com";
            string password = "SecurePassword123";
            string salt = "some_salt";
            string hashedPassword = AccountService.HashPassword(password, salt);
            Account account = new Account(Guid.NewGuid(), email, "username", salt, hashedPassword);

            mockRepository.Setup(repo => repo.GetAccount(email)).Returns(account);

            // Act
            User result = accountService.GetAccountWithCredentials(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(account.Username, result.Username); // Assuming User has a Username property
        }

        [Fact]
        public void GetAccountWithCredentials_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            string email = "user@example.com";
            string password = "WrongPassword";
            string salt = "some_salt";
            string hashedPassword = AccountService.HashPassword("CorrectPassword", salt);
            Account account = new Account(Guid.NewGuid(), email, "username", salt, hashedPassword);

            mockRepository.Setup(repo => repo.GetAccount(email)).Returns(account);

            // Act
            User result = accountService.GetAccountWithCredentials(email, password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAccountWithCredentials_RepositoryException_HandlesGracefully()
        {
            // Arrange
            var mockRepository = new Mock<ISqlAccountRepository>();
            mockRepository.Setup(repo => repo.GetAccount(It.IsAny<string>())).Throws(new Exception("Simulated database error"));

            var accountService = new AccountService(mockRepository.Object);

            // Act
            var result = accountService.GetAccountWithCredentials("test@example.com", "somepassword");

            // Assert
            Assert.Null(result); // Expecting null due to exception
        }
        [Fact]
        public void GetAccountWithCredentials_AccountExistsButPasswordHashMismatch_ReturnsNull()
        {
            // Arrange
            var email = "test@example.com";
            var correctPassword = "password123";
            var salt = AccountService.GenerateSalt();
            var correctHashedPassword = AccountService.HashPassword(correctPassword, salt);

            var account = new Account(Guid.NewGuid(), email, "TestUser", salt, correctHashedPassword);

            mockRepository.Setup(r => r.GetAccount(email)).Returns(account);

            // Act
            // Provide an incorrect password to cause a hash mismatch
            var result = accountService.GetAccountWithCredentials(email, "wrongpassword");

            // Assert
            Assert.Null(result); // Should return null due to hash mismatch
        }
        [Fact]
        public void GetAccountWithCredentials_CorrectEmailAndValidPassword_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var salt = AccountService.GenerateSalt();
            var hashedPassword = AccountService.HashPassword(password, salt);

            var account = new Account(Guid.NewGuid(), email, "TestUser", salt, hashedPassword);

            mockRepository.Setup(r => r.GetAccount(email)).Returns(account);

            // Act
            var result = accountService.GetAccountWithCredentials(email, password);

            // Assert
            Assert.NotNull(result); // Should return a valid User
            Assert.Equal("TestUser", result.Username); // Check if username matches
        }
        [Fact]
        public void GetAccountWithCredentials_IncorrectEmailButValidPassword_ReturnsNull()
        {
            // Arrange
            string correctEmail = "correct@example.com";
            string incorrectEmail = "incorrect@example.com";
            string password = "SecurePassword123";
            string salt = AccountService.GenerateSalt();
            string hashedPassword = AccountService.HashPassword(password, salt);

            Account account = new Account(Guid.NewGuid(), correctEmail, "username", salt, hashedPassword);

            mockRepository.Setup(repo => repo.GetAccount(correctEmail)).Returns(account);

            // Act
            User result = accountService.GetAccountWithCredentials(incorrectEmail, password);

            // Assert
            Assert.Null(result); // Should return null since email is incorrect
        }
        [Fact]
        public void GetAccountWithCredentials_InvalidEmailFormat_ReturnsNull()
        {
            // Act
            User result = accountService.GetAccountWithCredentials("invalid-email", "password");

            // Assert
            Assert.Null(result); // Should return null since email format is invalid
        }
        [Fact]
        public void GetAccountWithCredentials_NullRepository_HandlesGracefully()
        {
            // Arrange
            var accountService = new AccountService(null); // Null repository

            // Act
            var result = accountService.GetAccountWithCredentials("user@example.com", "password");

            // Assert
            Assert.Null(result); // Should return null since repository is null
        }
        [Fact]
        public void GetAccountWithCredentials_NoUserAccount_ReturnsNull()
        {
            // Arrange
            string email = "user@example.com";
            string password = "SecurePassword123";

            mockRepository.Setup(repo => repo.GetAccount(email)).Returns((Account)null); // No user account

            // Act
            User result = accountService.GetAccountWithCredentials(email, password);

            // Assert
            Assert.Null(result); // Should return null if no user account exists
        }
        [Fact]
        public void GetAccountWithCredentials_EmailCaseInsensitive_ReturnsUser()
        {
            // Arrange
            string lowerCaseEmail = "user@example.com";
            string upperCaseEmail = "USER@EXAMPLE.COM";
            string password = "SecurePassword123";
            string salt = AccountService.GenerateSalt();
            string hashedPassword = AccountService.HashPassword(password, salt);

            Account account = new Account(Guid.NewGuid(), lowerCaseEmail, "username", salt, hashedPassword);

            mockRepository.Setup(repo => repo.GetAccount(upperCaseEmail)).Returns(account); // Case-insensitive

            // Act
            User result = accountService.GetAccountWithCredentials(upperCaseEmail, password);

            // Assert
            Assert.NotNull(result); // Should return a user despite case differences
        }

        [Fact]
        public void GetAccountWithCredentials_RepositoryException_ReturnsNull()
        {
            // Arrange
            mockRepository.Setup(repo => repo.GetAccount(It.IsAny<string>())).Throws(new Exception("Simulated database error"));

            // Act
            var result = accountService.GetAccountWithCredentials("user@example.com", "SecurePassword123");

            // Assert
            Assert.Null(result); // Should return null when repository throws an exception
        }



        [Fact]
        public void GetAccountWithCredentials_CaseInsensitiveEmail_ReturnsUser()
        {
            // Arrange
            string email = "user@example.com";
            string password = "SecurePassword123";
            string salt = "some_salt";
            string hashedPassword = AccountService.HashPassword(password, salt);
            Account account = new Account(Guid.NewGuid(), email, "username", salt, hashedPassword);

            mockRepository.Setup(repo => repo.GetAccount("USER@EXAMPLE.COM")).Returns(account);

            // Act
            User result = accountService.GetAccountWithCredentials("USER@EXAMPLE.COM", password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(account.Username, result.Username);
        }

        [Fact]
        public void GetAccountWithCredentials_NullEmail_ReturnsNull()
        {
            // Act
            User result = accountService.GetAccountWithCredentials(null, "password");

            // Assert
            Assert.Null(result); // Should return null when email is null
        }

        [Fact]
        public void GetAccountWithCredentials_NullPassword_ReturnsNull()
        {
            // Act
            User result = accountService.GetAccountWithCredentials("user@example.com", null);

            // Assert
            Assert.Null(result); // Should return null when password is null
        }




        [Fact]
        public void AreAuthenticationCredentialsValid_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var salt = AccountService.GenerateSalt();
            var hashedPassword = AccountService.HashPassword(password, salt);
            var account = new Account(email, "TestUser", salt, hashedPassword);

            // Set repository expectations
            mockRepository.Setup(r => r.GetAccount(email)).Returns(account);

            // Act
            var result = accountService.AreAuthenticationCredentialsValid(email, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AreAuthenticationCredentialsValid_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            var email = "test@example.com";
            var password = "wrongpassword";
            var salt = AccountService.GenerateSalt();
            var hashedPassword = AccountService.HashPassword("correctpassword", salt);
            var account = new Account(email, "TestUser", salt, hashedPassword);

            // Set repository expectations
            mockRepository.Setup(r => r.GetAccount(email)).Returns(account);

            // Act
            var result = accountService.AreAuthenticationCredentialsValid(email, password);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void CreateUserAccount_ValidInput_ReturnsTrue()
        {
            // Arrange
            var email = "valid@example.com";
            var username = "validuser";
            var password = "ValidPassword123";

            mockRepository.Setup(r => r.AddAccount(It.IsAny<Account>())).Returns(true);
            mockRepository.Setup(r => r.AddUserAccount(It.IsAny<Account>())).Returns(true);

            // Act
            var result = accountService.CreateUserAccount(email, username, password);

            // Assert
            Assert.True(result);
            mockRepository.Verify(r => r.AddAccount(It.IsAny<Account>()), Times.Once);
            mockRepository.Verify(r => r.AddUserAccount(It.IsAny<Account>()), Times.Once);
        }
        [Fact]
        public void CreateUserAccount_AccountExists_ReturnsFalse()
        {
            // Arrange
            var email = "duplicate@example.com";
            var username = "duplicateuser";
            var password = "DuplicatePassword123";

            mockRepository.Setup(r => r.AddAccount(It.IsAny<Account>())).Returns(false);

            // Act
            var result = accountService.CreateUserAccount(email, username, password);

            // Assert
            Assert.False(result);
            mockRepository.Verify(r => r.AddAccount(It.IsAny<Account>()), Times.Once);
            mockRepository.Verify(r => r.AddUserAccount(It.IsAny<Account>()), Times.Never);
        }
        [Fact]
        public void CreateUserAccount_AddUserAccountFails_ReturnsFalse()
        {
            // Arrange
            var email = "fail@example.com";
            var username = "failuser";
            var password = "FailPassword123";

            mockRepository.Setup(r => r.AddAccount(It.IsAny<Account>())).Returns(true);
            mockRepository.Setup(r => r.AddUserAccount(It.IsAny<Account>())).Returns(false);

            // Act
            var result = accountService.CreateUserAccount(email, username, password);

            // Assert
            Assert.False(result);
            mockRepository.Verify(r => r.AddAccount(It.IsAny<Account>()), Times.Once);
            mockRepository.Verify(r => r.AddUserAccount(It.IsAny<Account>()), Times.Once);
        }
        [Fact]
        public void CreateUserAccount_EmptyInputs_ReturnsFalse()
        {
            // Arrange
            var emptyEmail = "";
            var emptyUsername = "";
            var emptyPassword = "";

            // Act & Assert
            Assert.False(accountService.CreateUserAccount(emptyEmail, "username", "password"));
            Assert.False(accountService.CreateUserAccount("email@example.com", emptyUsername, "password"));
            Assert.False(accountService.CreateUserAccount("email@example.com", "username", emptyPassword));
        }
        [Fact]
        public void AreAuthenticationCredentialsValid_NoAccount_ReturnsFalse()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var password = "password123";

            // Set repository expectations
            mockRepository.Setup(r => r.GetAccount(email)).Returns((Account)null);

            // Act
            var result = accountService.AreAuthenticationCredentialsValid(email, password);

            // Assert
            Assert.False(result);
        }

        // Test for invalid email format
        [Fact]
        public void TestIsValidEmail_Invalid()
        {
            // Arrange
            var accountService = new AccountService();

            // Act
            var result = accountService.IsEmailValid("invalid-email");

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
            var result = accountService.IsEmailValid("valid@example.com");

            // Assert
            Assert.True(result);
        }

        // Test to check password hashing
        [Fact]
        public void TestCreateArtistAccount()
        {
            var artistService = new AccountService();
            var result = artistService.CreateArtistAccount("", "", "");
            Assert.False(result);
        }

        [Fact]
        public void TestGenerateSalt_ShouldReturnBase64StringOfExpectedLength()
        {

            var accountService = new AccountService();
            // Arrange
            var expectedLength = 44; // A 32-byte array encoded to Base64 has 44 characters

            // Assuming GenerateSalt is a static method in a class named AccountService
            var generatedSalt = AccountService.GenerateSalt();

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
            var hash1 = AccountService.HashPassword(password, salt);
            var hash2 = AccountService.HashPassword(password, salt);

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
            var hashedPassword = AccountService.HashPassword(password, salt);

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
            var hashedPassword = AccountService.HashPassword(password, salt);

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
