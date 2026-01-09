using Marketplace.Application.Dtos.Auth;

namespace Marketplace.Application.IServices.Auth
{
    public interface IAuthServices
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto , string role);
        Task<AuthResponseDto> LogInAsync(LogInDto logInDto);
    }
}
