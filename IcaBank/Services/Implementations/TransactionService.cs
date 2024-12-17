using IcaBank.Context;
using IcaBank.Models;
using IcaBank.Models.Enums;
using IcaBank.Services.Interfaces;
using IcaBank.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace IcaBank.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _appDbContext;
        private ILogger<TransactionService> _logger;
        private AppSettings _appSettings;
        private static string _ourBankSettlementAccount;
        private readonly IAccountService _accountService;


        public TransactionService(AppDbContext appDbContext, ILogger<TransactionService> logger, IOptions<AppSettings> appSettings,
            IAccountService accountService)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _appSettings = appSettings.Value;
            _ourBankSettlementAccount = _appSettings.OurBankSettlementAccount;
            _accountService = accountService;
        }

        public Response CreateNewTransaction(Transaction transaction)
        {
            Response response = new Response();
            _appDbContext.Transactions.Add(transaction);
            _appDbContext.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transação criada com sucesso.";
            response.Data = null;

            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var transaction = _appDbContext.Transactions.Where(x => x.TransactionDate == date).ToList();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transação encontrada com sucesso.";
            response.Data = transaction;
            return response;
        }

        public Response MakeDeposit(string accountNumber, decimal amount, string transactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(accountNumber, transactionPin);
            if (authUser == null)
                throw new ApplicationException("Credenciais inválidas");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);
                destinationAccount = _accountService.GetByAccountNumber(accountNumber);

                sourceAccount.CurrentAccountBalance -= amount;
                destinationAccount.CurrentAccountBalance += amount;

                if((_appDbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                        (_appDbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //Transação realizada com sucesso.
                    transaction.TransactionStatus = ETransStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transação realizada com sucesso.";
                    response.Data = null;
                }
                else
                {
                    //Erro ao realizar transação
                    transaction.TransactionStatus = ETransStatus.Failed;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Não foi possivel realizar a transação.";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro... => {ex.Message}");
            }
            //Mensagem ao realizar transação
            transaction.TransactionType = ETransType.Deposit;
            transaction.TransactionSourceAccount = _ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = accountNumber;
            transaction.TransactionAmount = amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"Nova transação de => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $", Para a conta => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}. " +
                $"Realizada na data => {transaction.TransactionDate}, " +
                $"com a quantia de => {transaction.TransactionAmount}. " +
                $"Tipo de transação => {transaction.TransactionType}." +
                $"Status da transação => {transaction.TransactionStatus}";

            //salvando no banco de dados
            _appDbContext.Transactions.Add( transaction );
            _appDbContext.SaveChanges();

            return response;
        }

        public Response MakeFundsTransfer(string fromAccount, string toAccount, string transactionPin, decimal amount)
        {
            //Realizando retirada de dinheiro
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //Primeiro checkamos se a conta é válida
            //Será necessário autenticar o UserService, então injetamos o _IUserService
            var authUser = _accountService.Authenticate(fromAccount, transactionPin);
            if (authUser == null)
                throw new ApplicationException("Credenciais inválidas");

            //Se a validação estiver OK
            try
            {

                sourceAccount = _accountService.GetByAccountNumber(fromAccount);
                destinationAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);

                sourceAccount.CurrentAccountBalance -= amount;
                destinationAccount.CurrentAccountBalance += amount;

                if ((_appDbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                        (_appDbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = ETransStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transação realizada com sucesso.";
                    response.Data = null;
                }
                else
                {
                    //Erro realizando transação
                    transaction.TransactionStatus = ETransStatus.Failed;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Não foi possivel realizar a transação.";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro... => {ex.Message}");
            }

            //Mensagem ao realizar transação
            transaction.TransactionType = ETransType.Transfer;
            transaction.TransactionSourceAccount = fromAccount;
            transaction.TransactionDestinationAccount = toAccount;
            transaction.TransactionAmount = amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"Nova transação de => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $", Para a conta => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}. " +
                $"Realizada na data => {transaction.TransactionDate}, " +
                $"com a quantia de => {transaction.TransactionAmount}. " +
                $"Tipo de transação => {transaction.TransactionType}." +
                $"Status da transação => {transaction.TransactionStatus}";

            //Salvando no banco de dados
            _appDbContext.Transactions.Add(transaction);
            _appDbContext.SaveChanges();

            return response;
        }
    

        public Response MakeWithdraw(string accountNumber, decimal amount, string transactionPin)
        {
            //Realizando transação de dinheiro
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //Primeiro checkamos se a conta é válida
            //Será necessário autenticar o UserService, então injetamos o _IUserService
            var authUser = _accountService.Authenticate(accountNumber, transactionPin);
            if (authUser == null)
                throw new ApplicationException("Credenciais inválidas");

            //Se a validação estiver OK
            try
            {

                sourceAccount = _accountService.GetByAccountNumber(accountNumber);
                destinationAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);

                sourceAccount.CurrentAccountBalance -= amount;
                destinationAccount.CurrentAccountBalance += amount;

                if ((_appDbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                        (_appDbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //Transação aprovada
                    transaction.TransactionStatus = ETransStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transação realizada com sucesso.";
                    response.Data = null;
                }
                else
                {
                    //Transação reprovada
                    transaction.TransactionStatus = ETransStatus.Failed;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Não foi possivel realizar a transação.";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro... => {ex.Message}");
            }

            //Mensagem ao realizar transação
            transaction.TransactionType = ETransType.Withdraw;
            transaction.TransactionSourceAccount = _ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = accountNumber;
            transaction.TransactionAmount = amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"Nova transação de => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $", Para a conta => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}. " +
                $"Realizada na data => {transaction.TransactionDate}, " +
                $"com a quantia de => {transaction.TransactionAmount}. " +
                $"Tipo de transação => {transaction.TransactionType}." +
                $"Status da transação => {transaction.TransactionStatus}";

            //Salvando no banco de dados
            _appDbContext.Transactions.Add(transaction);
            _appDbContext.SaveChanges();

            return response;
        }
    }
}
