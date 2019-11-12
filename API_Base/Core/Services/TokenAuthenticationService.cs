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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Value.Secret));
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
        
        public JwtSecurityToken ValidateToken(string token){
            if(token == "" || token == null)
                return null;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters(){
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =  new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenManagement.Value.Secret)),
                ValidIssuer = _tokenManagement.Value.Issuer,
                ValidAudience = _tokenManagement.Value.Audience,
                ValidateIssuer = false,
                ValidateAudience = false
            };
            try
            {
                ClaimsPrincipal claim = new JwtSecurityTokenHandler()
                    .ValidateToken(token, validationParameters, out validatedToken); 
                
                return (JwtSecurityToken)validatedToken;
                    
            }
            catch (SecurityTokenValidationException stvex)
            {
                throw new Exception($"Token failed validation: {stvex.Message}");
            }            
            catch (ArgumentException argex)
            {
                throw new Exception($"Token was invalid: {argex.Message}");
            }
        }
    }
}