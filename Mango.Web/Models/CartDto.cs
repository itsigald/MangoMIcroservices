namespace Mango.Web.Models
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; } = new();
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
        public EmailInfoDto EmailInfoDto { get; set; } = new();
    }
}
