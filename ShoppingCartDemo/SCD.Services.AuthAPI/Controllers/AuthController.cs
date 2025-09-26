using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SCD.Services.AuthAPI.Models.Dto;
using SCD.Services.AuthAPI.Service.IService;

namespace SCD.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ResponseDto _responseDto;
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _responseDto = new ResponseDto();
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var errorMessage = await _authService.Register(registrationRequestDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = errorMessage;
                return Ok(_responseDto);
            }
            return Ok(_responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginresponse = await _authService.Login(loginRequestDto);
            if (loginresponse.User == null)
            {
                _responseDto.IsSuccess =false;
                _responseDto.Message = "Email or password is incorrect.";
                return BadRequest(_responseDto);
            }
            _responseDto.Result = loginresponse;
            return Ok(_responseDto);
        }
    }
}
