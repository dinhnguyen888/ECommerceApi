using ECommerceApi.Interfaces;
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
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                {
                    return BadRequest("Page and pageSize must be greater than 0.");
                }

                var (accounts, totalAccounts) = await _accountService.GetAccountsAsync(page, pageSize);
                var totalPages = (int)Math.Ceiling(totalAccounts / (double)pageSize);

                if (page > totalPages && totalPages > 0)
                {
                    return NotFound("Page number exceeds total pages.");
                }

                return Ok(new
                {
                    currentPage = page,
                    pageSize,
                    totalAccounts,
                    totalPages,
                    accounts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving accounts.", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAccountById(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
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
