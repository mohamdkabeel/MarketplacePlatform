using Marketplace.Application.Dtos.Auth;
using Marketplace.Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlatform.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LogInDto logInDto)
        {
            var result = await _authService.LogInAsync(logInDto);
            return Ok(result);
        }

    }
}
