using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app;
using app.src.SqlDataStorageAndRetrival;
using Moq; // for mocking dependencies

namespace MusicAppTests
{
    public class AccountServiceTests
    {
        // Test to check if a new user account is created successfully
        [Fact]
        public void TestCreateUserAccount_Success()
        {
            // Arrange
            var mockSqlAccountService = new Mock<SqlAccountService>();
            mockSqlAccountService.Setup(s => s.AddAccount(It.IsAny<Account>())).Returns(true);
            mockSqlAccountService.Setup(s => s.AddUserAccount(It.IsAny<Account>())).Returns(true);

            var accountService = new AccountService();

            // Act
            var result = accountService.CreateUserAccount("test@example.com", "testuser", "Test1234!");

            // Assert
            Assert.False(result);
        }

        // Test to check if creating a user account with existing email fails
        [Fact]
        public void TestCreateUserAccount_Failure()
        {
            // Arrange
            var mockSqlAccountService = new Mock<SqlAccountService>();
            mockSqlAccountService.Setup(s => s.AddAccount(It.IsAny<Account>())).Returns(false);

            var accountService = new AccountService();

            // Act
            var result = accountService.CreateUserAccount("existing@example.com", "existinguser", "Existing123!");

            // Assert
            Assert.False(result);
        }

        // Test to check user authentication
        [Fact]
        public void TestAuthenticate_ValidCredentials()
        {
            // Arrange
            var account = new Account("user@example.com", "testuser", "somesalt", "hashedpassword");
            var mockSqlAccountService = new Mock<SqlAccountService>();
            mockSqlAccountService.Setup(s => s.GetAccount("user@example.com")).Returns(account);

            var accountService = new AccountService();

            // Act
            var result = accountService.Authenticate("user@example.com", "password");

            // Assert
            Assert.True(result);
        }

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
      
    }

}
