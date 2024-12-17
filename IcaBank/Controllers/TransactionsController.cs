using AutoMapper;
using IcaBank.Models;
using IcaBank.Models.DTOs;
using IcaBank.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace IcaBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private IMapper _mapper;

        public TransactionsController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create_new_transaction")]
        public IActionResult CreateNewTransaction([FromBody] TransactionRequestDTO transactionRequest)
        {
            if(!ModelState.IsValid) 
                return BadRequest(transactionRequest);

            var transaction = _mapper.Map<Transaction>(transactionRequest);
            return Ok(_transactionService.CreateNewTransaction(transaction));
        }

        [HttpPost]
        [Route("make_deposit")]
        public IActionResult MakeDeposit(string accountNumber, decimal amount, string transactionPin)
        {
            if (!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Número da conta deve possuir 10 digitos.");
            return Ok(_transactionService.MakeDeposit(accountNumber, amount, transactionPin));
        }

        [HttpPost]
        [Route("make_withdraw")]
        public IActionResult MakeWithdraw(string accountNumber, decimal amount, string transactionPin)
        {
            if (!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Número da conta deve possuir 10 digitos.");
            return Ok(_transactionService.MakeWithdraw(accountNumber, amount, transactionPin));
        }

        [HttpPost]
        [Route("make_funds_transfer")]
        public IActionResult MakeFundsTransfer(string fromAccount, string toAccount, string transactionPin, decimal amount)
        {
            if (!Regex.IsMatch(fromAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$") || !Regex.IsMatch(toAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Número da conta deve possuir 10 digitos.");

            return Ok(_transactionService.MakeFundsTransfer(fromAccount, toAccount, transactionPin, amount));
        }
    }
}
