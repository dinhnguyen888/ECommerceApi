using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(string id);
        Task<Category> CreateCategoryAsync(CategoryPostDto categoryDto);
        Task<Category> UpdateCategoryAsync(string id, CategoryUpdateDto categoryDto);
        Task<bool> DeleteCategoryAsync(string id);
    }
}
