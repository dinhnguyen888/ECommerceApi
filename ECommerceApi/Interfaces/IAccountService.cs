using ECommerceApi.Dtos;
using ECommerceApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApi.Interfaces
{
    public interface IAccountService
    {
        Task<(List<AccountGetDto> accounts, int totalAccounts)> GetAccountsAsync(int page, int pageSize);
        Task<AccountGetDto> GetAccountByIdAsync(Guid id);
        Task<AccountGetDto> CreateAccountAsync(AccountPostDto accountDto);
        Task<AccountGetDto> UpdateAccountAsync(Guid id, AccountUpdateDto accountDto);
        Task<bool> DeleteAccountAsync(Guid id);
        Task<Account> CheckLegitAccount(string email);
       
    }
}
