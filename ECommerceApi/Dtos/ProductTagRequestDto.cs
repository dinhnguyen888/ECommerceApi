namespace ECommerceApi.Dtos
{
    public class ProductTagRequestDto
    {
        public string Tag { get; set; } = string.Empty;
        public int Limit { get; set; }
    }
}
