using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Uploader_Api.Helpers
{
    public class JwtSecurityHandlerWrapper
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IConfiguration _config;

        public JwtSecurityHandlerWrapper(IConfiguration config)
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _config = config;
        }

        public ClaimsPrincipal ValidateJwtToken(string token)
        {

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config.GetSection("Jwt:Issuer").Value,
                    ValidAudience = _config.GetSection("Jwt:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value))
                }, out SecurityToken validatedToken);

                return claimsPrincipal;
            }
            catch (SecurityTokenExpiredException)
            {
                throw new SecurityTokenExpiredException("Token has expired.");
            }
        }
    }
}
