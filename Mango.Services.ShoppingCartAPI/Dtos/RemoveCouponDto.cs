namespace Mango.Services.ShoppingCartAPI.Dtos
{
    public class RemoveCouponDto
    {
        public int CartHeaderId { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}
