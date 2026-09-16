using ECommerceApi.Models;

namespace ECommerceApi.Interfaces
{
    public interface ICartService
    {
        Task<List<Cart>> GetCartByUserId(string userId);
        Task<Cart> AddToCart(Cart cart);
        Task<bool> RemoveFromCart(string id);
        Task<bool> ClearCart(string userId);
    }
}
