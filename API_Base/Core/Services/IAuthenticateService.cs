using Microsoft.IdentityModel.Tokens;
using API_Base.Core.Models;

namespace API_Base.Core.Services
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(TokenRequest request, out string token);
    }
}