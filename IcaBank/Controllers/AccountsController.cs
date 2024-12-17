using AutoMapper;
using IcaBank.Models;
using IcaBank.Models.DTOs;
using IcaBank.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IcaBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register_new_account")]
        public IActionResult RegisterNewAccount([FromBody] RegisterNewAccountDTO newAccount)
        {
            if(!ModelState.IsValid)
                return BadRequest(newAccount);
            var account = _mapper.Map<Account>(newAccount);
            return Ok(_accountService.Create(account, newAccount.Pin, newAccount.ConfirmPin));
        }

        [HttpGet]
        [Route("get_all_accounts")]
        public IActionResult GetAllAccount() 
        {
            var accounts = _accountService.GetAllAccounts();
            var cleanesAccounts = _mapper.Map<IList<GetAccountDTO>>(accounts);
            return Ok(cleanesAccounts);
        }

        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(model);

            return Ok(_accountService.Authenticate(model.AccountNumber, model.Pin));
        }

        [HttpGet]
        [Route("get_by_account_number")]
        public IActionResult GetAccountByNumber(string accountNumber)
        {
            if (!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Número da conta precisa de 10 digitos.");

            var account = _accountService.GetByAccountNumber(accountNumber);
            var cleanedAccount = _mapper.Map<GetAccountDTO>(account);
            return Ok(cleanedAccount);
        }

        [HttpGet]
        [Route("get_account_by_id")]
        public IActionResult GetAccountById(int id)
        {
            var account = _accountService.GetById(id);
            var cleanedAccount = _mapper.Map<GetAccountDTO>(account);
            return Ok(cleanedAccount);
        }

        [HttpPut]
        [Route("update_account")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);
            var account = _mapper.Map<Account>(model);
            _accountService.Update(account, model.Pin);
            return Ok(account);
        }
    }
}
