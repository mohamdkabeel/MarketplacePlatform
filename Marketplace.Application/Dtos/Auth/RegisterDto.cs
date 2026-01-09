using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.Dtos.Auth
{
    public record RegisterDto(string? FirstName, string LastName, string? Email, string Password);
}
