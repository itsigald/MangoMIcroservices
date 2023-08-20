using Mango.Web.Interfaces;
using Mango.Web.Models;
using System.Runtime;

namespace Mango.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _service;
        private readonly IAppSettings _settings;

        public AuthService(IBaseService service, IAppSettings settings)
        {
            _service = service;
            _settings = settings;
        }

        public async Task<ResponseDto?> AssignRoleAsync(AssignRoleDto assignRole)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.POST,
                Data = assignRole,
                Url = $"{_settings.AuthUrlBase}{_settings.AuthAPI}/assignRole"
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.POST,
                Data = loginRequest,
                Url = $"{_settings.AuthUrlBase}{_settings.AuthAPI}/login"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequest)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.POST,
                Data = registrationRequest,
                Url = $"{_settings.AuthUrlBase}{_settings.AuthAPI}/register"
            }, withBearer: false);
        }
    }
}
