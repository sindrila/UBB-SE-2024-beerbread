using Moq;
using app.MVVM.Model.Domain;
using app.MVVM.Model.Data.SqlCommandHandlers;
using app.MVVM.Model.Data.Repositories;

namespace MusicAppTests
{
    public class SqlAccountRepositoryTests
    {
        private Account GetSampleAccount()
        {
            return new Account
            (
                Guid.NewGuid(),
                "test@example.com",
                "test_user",
                "random_salt",
               "hashed_password"
            );
        }

        [Fact]
        public void SqlAccountRepository_EmptyConstructor()
        {
            var sqlAccountRepository = new SqlAccountRepository();
            Assert.IsType<SqlAccountRepository>(sqlAccountRepository);
        }

        [Fact]
        public void AddAccount_ValidAccount_ReturnsTrue()
        {
            // Arrange
            var mockCommandExecutor = new Mock<ISqlAccountTableCommandExecutor>();
            mockCommandExecutor.Setup(e => e.ExecuteInsertCommandForAccount(It.IsAny<Account>())).Returns(true);
            var accountRepository = new SqlAccountRepository(mockCommandExecutor.Object);
            Account account = GetSampleAccount();

            // Act
            bool result = accountRepository.AddAccount(account);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AddUserAccount_ValidAccount_ReturnsTrue()
        {
            // Arrange
            var mockCommandExecutor = new Mock<ISqlAccountTableCommandExecutor>();
            mockCommandExecutor.SetupSequence(e => e.GetDatabaseIdAndGuidForAccountWithEmail(It.IsAny<string>()))
                               //.Returns((-1, null))
                               .Returns((1, "guid"));
            mockCommandExecutor.Setup(e => e.ExecuteCreateHistoryLikedBlockedPlaylistsForAccount(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            mockCommandExecutor.Setup(e => e.ExecuteCreateUserAccount(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            var accountRepository = new SqlAccountRepository(mockCommandExecutor.Object);
            Account account = GetSampleAccount();

            // Act
            bool result = accountRepository.AddUserAccount(account);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddUserAccount_GetDatabaseIdFails_ReturnsFalse()
        {
            // Arrange
            var mockCommandExecutor = new Mock<ISqlAccountTableCommandExecutor>();
            mockCommandExecutor.Setup(e => e.GetDatabaseIdAndGuidForAccountWithEmail(It.IsAny<string>())).Returns((-1, null));
            var accountRepository = new SqlAccountRepository(mockCommandExecutor.Object);
            Account account = GetSampleAccount();

            // Act
            bool result = accountRepository.AddUserAccount(account);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddUserAccount_CreatePlaylistsFails_ReturnsFalse()
        {
            // Arrange
            var mockCommandExecutor = new Mock<ISqlAccountTableCommandExecutor>();
            mockCommandExecutor.SetupSequence(e => e.GetDatabaseIdAndGuidForAccountWithEmail(It.IsAny<string>()))
                .Returns((1, "guid"));
            mockCommandExecutor.Setup(e => e.ExecuteCreateHistoryLikedBlockedPlaylistsForAccount(It.IsAny<int>(), It.IsAny<string>())).Returns(false);
            var accountRepository = new SqlAccountRepository(mockCommandExecutor.Object);
            Account account = GetSampleAccount();

            // Act
            bool result = accountRepository.AddUserAccount(account);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddUserAccount_CreateUserAccountFails_ReturnsFalse()
        {
            // Arrange
            var mockCommandExecutor = new Mock<ISqlAccountTableCommandExecutor>();
            mockCommandExecutor.SetupSequence(e => e.GetDatabaseIdAndGuidForAccountWithEmail(It.IsAny<string>()))
                .Returns((1, "guid"));
            mockCommandExecutor.SetupSequence(e => e.ExecuteCreateHistoryLikedBlockedPlaylistsForAccount(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(true);
            mockCommandExecutor.Setup(e => e.ExecuteCreateUserAccount(It.IsAny<int>(), It.IsAny<string>())).Returns(false);
            var accountRepository = new SqlAccountRepository(mockCommandExecutor.Object);
            Account account = GetSampleAccount();

            // Act
            bool result = accountRepository.AddUserAccount(account);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetAccount_ValidEmail_ReturnsAccount()
        {
            // Arrange
            var mockCommandExecutor = new Mock<ISqlAccountTableCommandExecutor>();
            var expectedAccount = GetSampleAccount();
            mockCommandExecutor.Setup(e => e.GetAccountWithEmail(It.IsAny<string>())).Returns(expectedAccount);
            var accountRepository = new SqlAccountRepository(mockCommandExecutor.Object);
            string email = "example@example.com";

            // Act
            Account? result = accountRepository.GetAccount(email);

            // Assert
            Assert.Equal(expectedAccount, result);
        }
    }
}