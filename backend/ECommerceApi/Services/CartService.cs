using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using MongoDB.Driver;

namespace ECommerceApi.Services
{
    public class CartService : ICartService
    {
        private readonly IMongoCollection<Cart> _carts;

        public CartService(MongoDbContext dbContext)
        {
            _carts = dbContext.GetCollection<Cart>("Cart");
        }

        public async Task<List<Cart>> GetCartByUserId(string userId)
        {
            return await _carts.Find(cart => cart.UserId == userId).ToListAsync();
        }

        public async Task<Cart> AddToCart(Cart cart)
        {
            cart.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            cart.AddToCartAt = DateTime.UtcNow;

            //check if the product is already in the user's cart
            var existingCart = await _carts.Find(c => c.UserId == cart.UserId && c.ProductId == cart.ProductId).FirstOrDefaultAsync();
            //if the product is already in the cart, throw exception
            if (existingCart != null)
            {
                throw new Exception("Product already in cart.");
            }

            await _carts.InsertOneAsync(cart);
            return cart;
        }

        public async Task<bool> RemoveFromCart(string id)
        {
            var result = await _carts.DeleteOneAsync(cart => cart.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var result = await _carts.DeleteManyAsync(cart => cart.UserId == userId);
            return result.DeletedCount > 0;
        }
    }
}
