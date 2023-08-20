using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Dtos;
using Mango.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator tokenGenerator)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = registrationRequestDto.Name,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber,
                InternalId = registrationRequestDto.InternalId
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

                if (result.Succeeded)
                { 
                    var userToReturn = _context.ApplicationUsers.FirstOrDefault(u => u.Email == registrationRequestDto.Email);

                    if(userToReturn != null)
                    {
                        UserDto userDto = new UserDto
                        {
                            Id = userToReturn.Id,
                            Email = userToReturn.Email!,
                            Name = userToReturn.Name,
                            InternalId = userToReturn.InternalId,
                            PhoneNumber = userToReturn.PhoneNumber ?? string.Empty
                        };

                        return string.Empty;
                    }
                }
                else
                {
                    return result.Errors.FirstOrDefault()?.Description;
                }
            }
            catch (Exception)
            {

            }

            return "General Error Encountered";
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto requestDto)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.InternalId == requestDto.Username);

            if (user != null)
            {
                var isPwdValid = await _userManager.CheckPasswordAsync(user, requestDto.Password);

                if (!isPwdValid)
                {
                    return new LoginResponseDto
                    {
                        User = null,
                        Token = string.Empty
                    };
                }

                UserDto userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    InternalId = user.InternalId,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber ?? string.Empty
                };

                var roles = await _userManager.GetRolesAsync(user);

                LoginResponseDto loginResponse = new LoginResponseDto
                {
                    User = userDto,
                    Token = _tokenGenerator.GenerateToken(user, roles),
                };

                return loginResponse;
            }
            else
            {
                return new LoginResponseDto
                {
                    User = null,
                    Token = string.Empty
                };
            }
        }

        public async Task<bool> AssunRole(string internalId, string roleName)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.InternalId == internalId);

            if (user != null)
            {
                // se non ho un metodo async posso usare questa sintassi:
                // if(_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                if (!(await _roleManager.RoleExistsAsync(roleName)))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await _userManager.AddToRoleAsync(user, roleName);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
