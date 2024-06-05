using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Uploader_Api.Dtos;
using Uploader_Api.Models;
using Uploader_Api.Services.Contract;

namespace Uploader_Api.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        public async Task<IdentityResult> RegisterUser(UserDto user)
        {
            var userCount = await _userManager.Users.CountAsync();
            var checkUser = await _userManager.FindByEmailAsync(user.email);
            if (checkUser == null)
            {
                var NewUser = new User
                {
                    firstname = user.firstname,
                    lastname = user.lastname,
                    userEmail = user.email,
                    UserName = user.email.Split("@")[0],
                    Email = user.email,


                };
                var results = await _userManager.CreateAsync(NewUser, user.password);
                var addToRoles = userCount < 1 ? await _userManager.AddToRoleAsync(NewUser , "Admin") : await _userManager.AddToRoleAsync(NewUser, "User");
                if (results.Succeeded && addToRoles.Succeeded)
                {
                    return IdentityResult.Success;
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError { Description = "failed registeration." });

                }
            }
            else
            {
                return IdentityResult.Failed(new IdentityError { Description = "User with this email already exists.", Code = 409.ToString() });
            }

        }

        public async Task<IdentityResult> LoginUser(LoginDto user)
        {

            var checkUser = await _userManager.FindByEmailAsync(user.email);
            if (checkUser != null)
            {

                var passwordChecker = await _userManager.CheckPasswordAsync(checkUser, user.password);
                if (passwordChecker)
                {
                    return IdentityResult.Success;
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Incorrect Password", Code = "404" });

                }


            }
            else
            {
                return IdentityResult.Failed(new IdentityError { Description = "You have not Registered, Please do", Code= "404" });
            }

        }

        public async Task<string> GenerateToken(LoginDto user)
        {
            var userFromDb = await _userManager.FindByEmailAsync(user.email);
            if (userFromDb == null)
            {
                // User not found
                throw new ApplicationException("User not found.");
            }

            var userRoles = await _userManager.GetRolesAsync(userFromDb);
            List<Claim> claims = new List<Claim> {
                new(ClaimTypes.Email, user.email),
                new(ClaimTypes.Role, string.Join(",", userRoles)),

            };
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            SigningCredentials signincred = new(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                signingCredentials: signincred
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return tokenString;

        }
    }
}
