using Microsoft.AspNetCore.Identity;
using Uploader_Api.Dtos;

namespace Uploader_Api.Services.Contract
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUser(UserDto user);
        Task<IdentityResult> LoginUser(LoginDto user);
        Task<string> GenerateToken(LoginDto user);

    }
}