using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppTests
{
    using Xunit;
    using app.MVVM.Model.Domain;
    using app.MVVM.ViewModel;
    using Moq;

    public class LoginViewModelTests
    {
        private readonly Mock<IAccountService> mockAccountService;
        private readonly LoginViewModel loginViewModel;

        public LoginViewModelTests()
        {
            mockAccountService = new Mock<IAccountService>();
            loginViewModel = new LoginViewModel(this.mockAccountService.Object);
        }
        // Test default constructor
        [Fact]
        public void LoginViewModel_DefaultConstructor_InitializesAccountService()
        {
            var loginViewModel = new LoginViewModel();
            Assert.NotNull(loginViewModel); 
            var accountServiceField = typeof(LoginViewModel).GetField("accountService", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var accountService = accountServiceField.GetValue(loginViewModel);
            Assert.NotNull(accountService); 
            Assert.IsType<AccountService>(accountService); 
        }
        // Test setting AccountService
        [Fact]
        public void AccountService_Setter_SetsCorrectValue()
        {
            var loginViewModel = new LoginViewModel();
            var mockAccountService = new Mock<IAccountService>();
            this.loginViewModel.AccountService = this.mockAccountService.Object;
            Assert.Equal(this.mockAccountService.Object, this.loginViewModel.AccountService); 
        }
        // Test getting AccountService after setting it
        [Fact]
        public void AccountService_Getter_ReturnsSetValue()
        {
            var loginViewModel = new LoginViewModel();
            var mockAccountService = new Mock<IAccountService>();
            this.loginViewModel.AccountService = this.mockAccountService.Object; 
            var accountService = this.loginViewModel.AccountService;
            Assert.Equal(this.mockAccountService.Object, accountService); 
        }
        // Test default AccountService value
        [Fact]
        public void AccountService_DefaultConstructor_IsNull()
        { 
            var loginViewModel = new LoginViewModel();
            var accountService = this.loginViewModel.AccountService;
            Assert.Null(accountService); 
        }
        // Test for valid email format
        [Fact]
        public void IsEmailValid_ValidEmail_ReturnsTrue()
        {
            string validEmail = "user@example.com";
            this.mockAccountService.Setup(service => service.IsEmailValid(validEmail)).Returns(true);
            bool result = this.loginViewModel.IsEmailValid(validEmail);
            Assert.True(result); 
        }
        // Test for invalid email format
        [Fact]
        public void IsEmailValid_InvalidEmail_ReturnsFalse()
        {
            string invalidEmail = "invalid-email";
            this.mockAccountService.Setup(service => service.IsEmailValid(invalidEmail)).Returns(false);
            bool result = this.loginViewModel.IsEmailValid(invalidEmail);
            Assert.False(result); 
        }
        // Test for null email
        [Fact]
        public void IsEmailValid_NullEmail_ReturnsFalse()
        {
            bool result = this.loginViewModel.IsEmailValid(null);
            Assert.False(result); 
        }
        // Test for empty email
        [Fact]
        public void IsEmailValid_EmptyEmail_ReturnsFalse()
        {
            bool result = this.loginViewModel.IsEmailValid("");
            Assert.False(result); 
        }
        // Test for valid password
        [Fact]
        public void IsPasswordValid_ValidPassword_ReturnsTrue()
        {
            string validPassword = "SecurePassword123";
            bool result = this.loginViewModel.IsPasswordValid(validPassword);
            Assert.True(result); 
        }
        // Test for invalid password (too short)
        [Fact]
        public void IsPasswordValid_InvalidPassword_ReturnsFalse()
        {
            string invalidPassword = "12345"; 
            bool result = this.loginViewModel.IsPasswordValid(invalidPassword);
            Assert.False(result); 
        }
        // Test for null password
        [Fact]
        public void IsPasswordValid_NullPassword_ReturnsFalse()
        {
            bool result = this.loginViewModel.IsPasswordValid(null);
            Assert.False(result); 
        }
        // Test for empty password
        [Fact]
        public void IsPasswordValid_EmptyPassword_ReturnsFalse()
        {
            bool result = this.loginViewModel.IsPasswordValid("");
            Assert.False(result);
        }
        // Test for valid credentials
        [Fact]
        public void AreAuthenticationCredentialsValid_ValidCredentials_ReturnsTrue()
        {
            string email = "user@example.com";
            string password = "SecurePassword123";
            this.mockAccountService.Setup(service => service.AreAuthenticationCredentialsValid(email, password)).Returns(true);
            bool result = this.loginViewModel.AreAuthenticationCredentialsValid(email, password);
            Assert.True(result); 
        }
        // Test for invalid credentials
        [Fact]
        public void AreAuthenticationCredentialsValid_InvalidCredentials_ReturnsFalse()
        {
            string email = "user@example.com";
            string password = "WrongPassword";
            this.mockAccountService.Setup(service => service.AreAuthenticationCredentialsValid(email, password)).Returns(false);
            bool result = this.loginViewModel.AreAuthenticationCredentialsValid(email, password);
            Assert.False(result); 
        }       
        // Test for failed authentication
        [Fact]
        public void AuthenticateAndGetCurrentUser_InvalidCredentials_ReturnsNull()
        {
            string email = "user@example.com";
            string password = "WrongPassword";
            this.mockAccountService.Setup(service => service.GetAccountWithCredentials(email, password)).Returns((User)null);
            User result = this.loginViewModel.AuthenticateAndGetCurrentUser(email, password);
            Assert.Null(result); 
        }
    }

}
