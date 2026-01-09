using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.Dtos.Auth
{
    public record AuthResponseDto(string Token, string RefreshToken, string Email, string UserId);
}
