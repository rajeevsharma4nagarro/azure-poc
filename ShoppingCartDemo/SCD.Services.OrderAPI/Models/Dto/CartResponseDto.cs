namespace SCD.Services.OrderAPI.Models.Dto
{
    public class CartDetailsResponseDto
    {
        public int CartDetailId { get; set; }
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public ProductDto Product { get; set; }
    }

    public class CartResponseDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsResponseDto>? CartDetails { get; set; }
    }
}
