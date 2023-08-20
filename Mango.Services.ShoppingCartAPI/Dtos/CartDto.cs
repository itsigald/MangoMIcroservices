namespace Mango.Services.ShoppingCartAPI.Dtos
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; } = new();
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
