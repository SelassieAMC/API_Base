using System.IdentityModel.Tokens.Jwt;
using API_Base.Core.Models;

namespace API_Base.Core.Services
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(TokenRequest request, out string token);
        JwtSecurityToken ValidateToken(string token);
    }
}