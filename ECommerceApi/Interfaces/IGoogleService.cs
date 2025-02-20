using ECommerceApi.Dtos;

namespace ECommerceApi.Interfaces
{
    public interface IGoogleService
    {
        Task<string> GenerateTokenFromGoogleInfo(GoogleRequestDto googleRequestDto);
    }
}
