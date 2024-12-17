using IcaBank.Context;
using IcaBank.Models;
using IcaBank.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IcaBank.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _appDbContext;

        public AccountService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Account Authenticate(string accountNumber, string pin)
        {
            var account = _appDbContext.Accounts.Where(x => x.AccountNumberGenerated == accountNumber).SingleOrDefault();
            if (account == null)
                return null;
            if (!VerifyPinHash(pin, account.PinHash, account.PinSalt))
                return null;
            return account;
        }

        private static bool VerifyPinHash(string pin, byte[] pinHash, byte[] pinSalt)
        {
            if(string.IsNullOrWhiteSpace(pin))
                throw new ArgumentNullException("Pin");
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
                for(int i = 0; i< computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i]) return false;
                }
            }

            return true;
        }

        public Account Create(Account account, string pin, string confirmPin)
        {
            if (_appDbContext.Accounts.Any(x => x.Email == account.Email)) 
                throw new ApplicationException("Já existe uma conta com este e-mail.");
            if (!pin.Equals(confirmPin)) 
                throw new ArgumentException("O Pin não foi encontrado", "Pin");

            byte[] pinHash, pinSalt;
            CreatePinHash(pin, out pinHash, out pinSalt);
            
            account.PinHash = pinHash;
            account.PinSalt = pinSalt;

            _appDbContext.Accounts.Add(account);
            _appDbContext.SaveChanges();

            return account;
        }

        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        public void Delete(int id)
        {
            var account = _appDbContext.Accounts.Find(id);
            if(account != null)
            {
                _appDbContext.Accounts.Remove(account);
                _appDbContext.SaveChanges();
            }
        }

        public Account GetById(int id)
        {
            var account = _appDbContext.Accounts.Where(x => x.ID == id).FirstOrDefault();
            if (account == null) return null;

            return account;
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _appDbContext.Accounts.ToList();
        }

        public Account GetByAccountNumber(string accountNumber)
        {
            var account = _appDbContext.Accounts.Where(x => x.AccountNumberGenerated == accountNumber).FirstOrDefault();
            if(account == null) return null;

            return account;
        }

        public void Update(Account account, string pin = null)
        {
            var accountToBeUpdated = _appDbContext.Accounts.Where(x => x.Email == account.Email).SingleOrDefault();
            if (accountToBeUpdated == null)
                throw new ApplicationException("A conta escolhida não existe.");
            if (!string.IsNullOrWhiteSpace(account.Email))
            {
                if (_appDbContext.Accounts.Any(x => x.Email == account.Email))
                    throw new ApplicationException("O Email: " + account.Email + ", já está cadastrado");
                
                accountToBeUpdated.Email = account.Email;
            }

            if (!string.IsNullOrWhiteSpace(account.Phone))
            {
                if (_appDbContext.Accounts.Any(x => x.Phone == account.Phone))
                    throw new ApplicationException("O Telefone: " + account.Email + ", já está cadastrado");

                accountToBeUpdated.Phone = account.Phone;
            }

            if (!string.IsNullOrWhiteSpace(pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(pin, out pinHash, out pinSalt);

                accountToBeUpdated.PinHash = pinHash;
                accountToBeUpdated.PinSalt = pinSalt;
            }
            accountToBeUpdated.DateLastUpdated = DateTime.UtcNow;

            _appDbContext.Accounts.Update(accountToBeUpdated);
            _appDbContext.SaveChanges();
        }
    }
}
