using System.Threading.Tasks;
using ECommerce.TransactionService.Application.Dtos;

namespace ECommerce.TransactionService.Application.Interfaces
{
    // Interface cho BuyNow Service
    public interface IBuyNowService
    {
        // Tao don hang buy-now va tra ve payment URL
        Task<BuyNowResponseDto> BuyNowAsync(BuyNowDto dto);
    }
}
