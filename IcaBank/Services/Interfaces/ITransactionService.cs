using IcaBank.Models;
using System;

namespace IcaBank.Services.Interfaces
{
    public interface ITransactionService
    {
        Response CreateNewTransaction(Transaction transaction);
        Response FindTransactionByDate(DateTime date);
        Response MakeDeposit(string accountNumber, decimal amount, string transactionPin);
        Response MakeWithdraw(string accountNumber, decimal amount, string transactionPin);
        Response MakeFundsTransfer(string fromAccount, string toAccount, string transactionPin, decimal amount);
    }
}
