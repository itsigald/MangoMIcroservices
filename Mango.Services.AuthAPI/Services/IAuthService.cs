using Mango.Services.AuthAPI.Dtos;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Mango.Services.AuthAPI.Services
{
    public interface IAuthService
    {
        Task<string> Register (RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login (LoginRequestDto requestDto);
        Task<bool> AssunRole(string internalId, string roleName);
    }
}
