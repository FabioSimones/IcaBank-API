using IcaBank.Models;
using System.Collections.Generic;

namespace IcaBank.Services.Interfaces
{
    public interface IAccountService
    {
        Account Authenticate(string accountNumber, string pin);
        IEnumerable<Account> GetAllAccounts();
        Account Create(Account account, string pin, string confirmPin);
        void Update(Account account, string pin = null);
        void Delete(int id);
        Account GetById(int id);
        Account GetByAccountNumber(string accountNumber);
    }
}
