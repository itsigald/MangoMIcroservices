using Mango.Web.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace Mango.Web.Models
{
    public class AppSettings : IAppSettings
    {
        private string? _couponUrlBase;
        private string? _couponAPI;

        private string? _authUrlBase;
        private string? _authAPI;

        private string? _productUrlBase;
        private string? _productAPI;

        private string? _shoppingCartUrlBase;
        private string? _shoppingCartAPI;

        private List<string> _roles;
        private string? _emailFrom;

        public AppSettings(Settings? settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(Settings));
            }

            _couponUrlBase = settings.CouponUrlBase;
            _couponAPI = settings.CouponAPI;

            _authUrlBase = settings.AuthUrlBase;
            _authAPI = settings.AuthAPI;

            _productUrlBase = settings.ProductUrlBase;
            _productAPI = settings.ProductAPI;

            _shoppingCartUrlBase = settings.ShoppingCartUrlBase;
            _shoppingCartAPI = settings.ShoppingCartAPI;

            _roles = settings.Roles;

            _emailFrom = settings.EmailFrom;
        }
        
        public string? CouponUrlBase => _couponUrlBase;
        public string? CouponAPI => _couponAPI;

        public string? AuthUrlBase => _authUrlBase;
        public string? AuthAPI => _authAPI;

        public string? ProductUrlBase => _productUrlBase;
        public string? ProductAPI => _productAPI;
        
        public string? ShoppingCartUrlBase => _shoppingCartUrlBase;
        public string? ShoppingCartAPI => _shoppingCartAPI;

        public List<string> Roles => _roles;

        public string? EmailFrom => _emailFrom;
    }
}
