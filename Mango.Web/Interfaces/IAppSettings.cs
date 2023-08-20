namespace Mango.Web.Interfaces
{
    public interface IAppSettings
    {
        string? CouponUrlBase { get; }
        string? CouponAPI { get; }

        string? AuthUrlBase { get; }
        string? AuthAPI { get; }

        string? ProductUrlBase { get; }
        string? ProductAPI { get; }

        string? ShoppingCartUrlBase { get; }
        string? ShoppingCartAPI { get; }

        List<string> Roles { get; } 
    }
}
