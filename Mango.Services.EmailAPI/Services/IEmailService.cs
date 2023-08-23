using Mango.Services.EmailAPI.Data;

namespace Mango.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto? cartDto);
    }
}
