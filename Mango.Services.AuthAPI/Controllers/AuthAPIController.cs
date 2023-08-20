using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Dtos;
using Mango.Services.AuthAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _context;
        protected ResponseDto _responseDto;

        public AuthAPIController(IAuthService authService, AppDbContext context)
        {
            _authService = authService;
            _context = context;
            _responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var error = await _authService.Register(registrationRequestDto);

            if(!string.IsNullOrEmpty(error))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = error;

                return BadRequest(_responseDto);
            }
            else
            {
                return Ok(_responseDto);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            /*
            Rgistrasto come ADMIN:
                username: 00000001
                password: Dansig@963

            Registrato come CUSTOMER:
                username: 00000001
                password: Cr0st0n3!
            */

            var loginResponse = await _authService.Login(loginDto);

            if(loginResponse.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "User or password incorrect";
                return BadRequest(_responseDto);
            }

            _responseDto.Result = loginResponse;
            return Ok(_responseDto);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssingRoleDto assignRoleDto)
        {
            var roleAdded = await _authService.AssunRole(assignRoleDto.InternalId, assignRoleDto.Role.ToUpper());

            if (roleAdded)
            {
                return Ok(_responseDto);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Role not added";
                return BadRequest(_responseDto);
            }
        }
    }
}
