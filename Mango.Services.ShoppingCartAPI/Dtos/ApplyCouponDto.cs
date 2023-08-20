namespace Mango.Services.ShoppingCartAPI.Dtos
{
    public class ApplyCouponDto
    {
        public int CartHeaderId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string? CouponCode { get; set; }
    }
}
