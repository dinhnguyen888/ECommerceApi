using ECommerceApi.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_e_commerce_website.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountGetDto>> GetAllAccountsAsync();
        Task<AccountGetDto> GetAccountByIdAsync(Guid id);
        Task<AccountGetDto> CreateAccountAsync(AccountPostDto accountDto);
        Task<AccountGetDto> UpdateAccountAsync(Guid id, AccountUpdateDto accountDto);
        Task<bool> DeleteAccountAsync(Guid id);
    }
}
