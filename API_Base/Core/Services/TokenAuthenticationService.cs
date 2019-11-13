using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_Base.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API_Base.Core.Services
{
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IOptions<TokenManagement> _tokenManagement;

        public TokenAuthenticationService(
            IUserManagementService userManagementService,
            IOptions<TokenManagement> tokenManagement
            )
        {
            _tokenManagement = tokenManagement;
            _userManagementService = userManagementService;
        }
        public bool IsAuthenticated(TokenRequest request, out string token)
        {
            token = string.Empty;
            if (!_userManagementService.IsValidUser(request.Username, request.Password)) 
                return false;

            var claim = new[]
            {
                new Claim(ClaimTypes.Name, request.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenManagement.Value.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Value.Issuer, //expected origin
                _tokenManagement.Value.Audience,
                claim,
                expires: DateTime.Now.AddMinutes(_tokenManagement.Value.AccessExpiration),
                signingCredentials: credentials
            );
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }
    }
}