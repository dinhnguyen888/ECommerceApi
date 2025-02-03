﻿using ECommerceApi.Interfaces;
using ECommerceApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAccountById(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();
            return Ok(account);
        }
        [HttpGet("profiles")]
        public async Task<IActionResult> GetAccountByToken([FromQuery]string token)
        {
            var account = await _accountService.GetAccountByToken(token);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountPostDto accountDto)
        {
            var account = await _accountService.CreateAccountAsync(accountDto);
            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] AccountUpdateDto accountDto)
        {
            var updatedAccount = await _accountService.UpdateAccountAsync(id, accountDto);
            if (updatedAccount == null) return NotFound();
            return Ok(updatedAccount);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var result = await _accountService.DeleteAccountAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
