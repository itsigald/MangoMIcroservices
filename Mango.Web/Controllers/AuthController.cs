using Mango.Web.Interfaces;
using Mango.Web.Models;
using Mango.Web.Services;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IAppSettings _appSettings;
        private readonly ITokenProvider _tokenProvider;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AuthController(IAuthService authService, IAppSettings appSettings, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _appSettings = appSettings;
            _tokenProvider = tokenProvider;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleSelect = new List<SelectListItem>();

            foreach (var role in _appSettings.Roles)
            {
                roleSelect.Add(new SelectListItem(role, role));
            }

            ViewBag.Roles = roleSelect;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationDto)
        {
            ResponseDto? responseRegistration = await _authService.RegisterAsync(registrationDto);

            if(responseRegistration != null && responseRegistration.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationDto.Role))
                    registrationDto.Role = _appSettings.Roles.Last();

                ResponseDto? responseAssignRole = await _authService.AssignRoleAsync(new AssignRoleDto
                {
                    InternalId = registrationDto.InternalId,
                    Role = registrationDto.Role,
                });

                if (responseAssignRole != null && responseAssignRole.IsSuccess)
                {
                    TempData["success"] = $"Registration for {registrationDto.InternalId} successfull";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = responseRegistration?.Message;
                return View(registrationDto);
            }

            var roleSelect = new List<SelectListItem>();

            foreach (var role in _appSettings.Roles)
            {
                roleSelect.Add(new SelectListItem(role, role));
            }

            TempData["error"] = $"Error on Registration for {registrationDto.Name}: {responseRegistration?.Message}";
            ViewBag.Roles = roleSelect;

            return View(registrationDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginDto)
        {
            ResponseDto? responseLogin = await _authService.LoginAsync(loginDto);

            if (responseLogin != null && responseLogin.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonSerializer.Deserialize<LoginResponseDto>(Convert.ToString(responseLogin.Result), _jsonSerializerOptions);
                
                if(loginResponseDto != null)
                {
                    await SignInUser(loginResponseDto);
                    _tokenProvider.SetToken(loginResponseDto.Token);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = responseLogin?.Message;
                    return View(loginDto);
                }
            }
            else
            {
                TempData["error"] = responseLogin?.Message;
                return View(loginDto);
            }

        }

        private async Task SignInUser(LoginResponseDto login)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(login.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.UniqueName, jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(c => c.Type == SD.RoleName).Value));

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
