using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using app.src.SqlDataStorageAndRetrival;
using app.Interfaces;

namespace app
{
    public class AccountService: Interfaces.IAccountService
    {
        private readonly ISqlAccountService sqlAccountService ;

        public AccountService(ISqlAccountService sqlAccountService)
        {
            this.sqlAccountService = sqlAccountService;
        }   
        public AccountService()
        {
            this.sqlAccountService = new SqlAccountService();
        }

        // Creation
        public bool CreateUserAccount(string email, string username, string password)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(password, salt);
            Account newAccount = new Account(email, username, salt, hashedPassword);
            if (!sqlAccountService.AddAccount(newAccount))
                return false;

            return sqlAccountService.AddUserAccount(newAccount);
        }
        public bool CreateArtistAccount(string email, string username, string password)
        {
            //todo
            return false;
        }
        // Authentication
        public bool Authenticate(string email, string password)
        {
            Account account = sqlAccountService.GetAccount(email);
            if (account == null)
                return false;

            string hashedPasswordAttempt = HashPassword(password, account.Salt);
            return account.VerifyPassword(hashedPasswordAttempt);
        }

        // Salt generator
        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        // Method to hash the password
        protected string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
