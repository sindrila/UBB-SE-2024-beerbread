﻿namespace app.MVVM.ViewModel
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using app.MVVM.Model.Data.Repositories;
    using app.MVVM.Model.Domain;

    public interface IAccountService
    {
        bool CreateUserAccount(string email, string username, string password);

        bool Authenticate(string email, string password);

        bool CreateArtistAccount(string email, string username, string password);

        bool IsValidEmail(string email);
    }

    public class AccountService : IAccountService
    {
        private readonly ISqlAccountRepository sqlAccountService;

        public AccountService()
        {
            sqlAccountService = new SqlAccountRepository();
        }

        public AccountService(ISqlAccountRepository sqlAccountRepository)
        {
            sqlAccountService = sqlAccountRepository;
        }

        // Creation
        public bool CreateUserAccount(string email, string username, string password)
        {
            string salt = AccountService.GenerateSalt();
            string hashedPassword = AccountService.HashPassword(password, salt);
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

        public bool Authenticate(string email, string password)
        {
            Account account = sqlAccountService.GetAccount(email);
            if (account == null)
            {
                return false;
            }

            string hashedPasswordAttempt = AccountService.HashPassword(password, account.Salt);
            return account.VerifyPassword(hashedPasswordAttempt);
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

        private static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
    }
}
