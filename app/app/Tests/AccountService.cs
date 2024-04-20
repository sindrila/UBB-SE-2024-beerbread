using NUnit.Framework;
using Moq;
using app.src.SqlDataStorageAndRetrival;

namespace app.Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        private Mock<SqlAccountService> mockSqlAccountService;
        private AccountService accountService;

        [SetUp]
        public void Setup()
        {
            mockSqlAccountService = new Mock<SqlAccountService>();
            accountService = new AccountService(mockSqlAccountService.Object);
        }

        [Test]
        public void CreateUserAccount_ValidInput_ReturnsTrue()
        {
            // Arrange
            string email = "test@example.com";
            string username = "testuser";
            string password = "password123";

            mockSqlAccountService.Setup(x => x.AddAccount(It.IsAny<Account>())).Returns(true);
            mockSqlAccountService.Setup(x => x.AddUserAccount(It.IsAny<Account>())).Returns(true);

            // Act
            bool result = accountService.CreateUserAccount(email, username, password);

            // Assert
            Assert.Equals(result,true);
        }

        [Test]
        public void CreateUserAccount_InvalidInput_ReturnsFalse()
        {
            // Arrange
            string email = "invalidemail";
            string username = "testuser";
            string password = "password123";

            // Act
            bool result = accountService.CreateUserAccount(email, username, password);

            // Assert
            Assert.Equals(result,false);
        }

        [Test]
        public void Authenticate_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            string email = "test@example.com";
            string password = "password123";

            Account testAccount = new Account(email, "testuser", "salt", "hashedPassword");
            mockSqlAccountService.Setup(x => x.GetAccount(email)).Returns(testAccount);

            // Act
            bool result = accountService.Authenticate(email, password);

            // Assert
            Assert.Equals(result,true);
        }

        [Test]
        public void Authenticate_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            string email = "test@example.com";
            string password = "wrongpassword";

            Account testAccount = new Account(email, "testuser", "salt", "hashedPassword");
            mockSqlAccountService.Setup(x => x.GetAccount(email)).Returns(testAccount);

            // Act
            bool result = accountService.Authenticate(email, password);

            // Assert
            Assert.Equals(result,false);
        }
    }
}
