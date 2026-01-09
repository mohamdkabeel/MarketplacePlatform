using Marketplace.Application.Dtos.Auth;
using Marketplace.Application.IServices.Auth;
using Marketplace.Core.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Marketplace.Infrastructure.Services.Auth
{
    public class AuthService : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager , IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto , string role = "Customer")
        {
            var user = new ApplicationUser
            {
               UserName = registerDto.Email,
               Email = registerDto.Email,
               FirstName = registerDto.FirstName,
               LastName = registerDto.LastName,
            };

            var result = await _userManager.CreateAsync(user ,registerDto.Password);
            if (!result.Succeeded) 
                throw new Exception(string.Join(", ", result.Errors.Select(e =>e.Description)));

            if (!await _userManager.IsInRoleAsync(user , role))
            {
                await _userManager.AddToRoleAsync(user , role); 
            }

            return await GenerateJwtToken(user);
        }


        public async Task<AuthResponseDto> LogInAsync(LogInDto logInDto) 
        {
            var user = await _userManager.FindByEmailAsync(logInDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user , logInDto.Password))
                throw new Exception("Invaild");

            return await GenerateJwtToken(user);
        }

        private async Task<AuthResponseDto> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub , user.Id),
                new Claim(JwtRegisteredClaimNames.Email , user.Email ),

                new Claim("FirstName" , user.FirstName),
                new Claim("LastName" , user.LastName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer : jwtSettings["Issure"],
                audience : jwtSettings["Audience"],
                claims: claims,
                expires : DateTime.Now.AddMinutes(jwtSettings.GetValue<int>("ExpiryMinutes")),
                signingCredentials : creds
                );

            return new AuthResponseDto(new JwtSecurityTokenHandler().WriteToken(token), Guid.NewGuid().ToString(), user.Email, user.Id);
        }

       
    }
}
