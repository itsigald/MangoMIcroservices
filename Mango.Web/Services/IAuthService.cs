using Mango.Web.Models;

namespace Mango.Web.Services
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequest);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequest);
        Task<ResponseDto?> AssignRoleAsync(AssignRoleDto assingRole);
    }
}
