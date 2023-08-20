namespace Mango.Web.Models
{
    public class Settings
    {
        public string CouponUrlBase { get; set; } = string.Empty;
        public string CouponAPI { get; set; } = string.Empty;

        public string AuthUrlBase { get; set; } = string.Empty;
        public string AuthAPI { get; set; } = string.Empty;

        public string ProductUrlBase { get; set; } = string.Empty;
        public string ProductAPI { get; set; } = string.Empty;

        public string ShoppingCartUrlBase { get; set; } = string.Empty;
        public string ShoppingCartAPI { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();
    }
}