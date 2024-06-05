using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Thinktecture.IdentityModel.Tokens;
using Uploader_Api.Helpers;
//using Thinktecture.IdentityModel.Tokens;
namespace Uploader_Api.Middleware
{
    public class AuthMiddleware : IMiddleware
    {
        private readonly IConfiguration _config;
        private readonly JwtSecurityHandlerWrapper _jwtSecurityTokenHandler;

        public AuthMiddleware( IConfiguration config, JwtSecurityHandlerWrapper jwtSecurityTokenHandler)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
            _config = config;   
        }

        //public ClaimsPrincipal ValidateJwtToken(string token)
        //{
          
        //    try
        //    {
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            ValidIssuer = _config.GetSection("Jwt:Issuer").Value,
        //            ValidAudience = _config.GetSection("Jwt:Audience").Value,
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value))
        //        }, out SecurityToken validatedToken);

        //        return claimsPrincipal;
        //    }
        //    catch (SecurityTokenExpiredException)
        //    {
        //        throw new SecurityTokenExpiredException("Token has expired.");
        //    }
        //}
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            Console.WriteLine(token);

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    // Verify the token using the JwtSecurityTokenHandlerWrapper
                    var claimsPrincipal = _jwtSecurityTokenHandler.ValidateJwtToken(token);
                    if (claimsPrincipal != null)
                    {
                        var userEmail = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

                        // Store the user ID in the HttpContext items for later use
                        context.Items["UserEmail"] = userEmail;

                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Unauthorized: forbidden");
                    }
                    // Extract the user ID from the token
                    
                }
                catch (Exception)
                {
                    // If the token is invalid, throw an exception
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Token is invalid or expired.");
                }
                await next(context);

            }
            //else
            //{
            //    // Token is required
            //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    await context.Response.WriteAsync("Unauthorized: Token is required.");
            //}

        }
    }
}
