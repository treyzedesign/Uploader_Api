using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Uploader_Api.Dtos;
using Uploader_Api.Services.Contract;

namespace Uploader_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) {
            _authService = authService;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(UserDto user)
        {
            var result = await _authService.RegisterUser(user);
             if (result.Succeeded)
            {
                return Ok(ApiResponse<bool>.SuccessResponse(true, "User registered successfully"));
            }
            else
            {
                return BadRequest(result.Errors);

            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser(LoginDto user)
        {
            var result = await _authService.LoginUser(user);
            //var see = JsonConvert.DeSerializeObject(result);
            if (result.Succeeded)
            {
                Console.WriteLine(result);
                return Ok(ApiResponse<bool>.LoginResponse(true, await _authService.GenerateToken(user), "User login successfully"));
            }
            else if (result.Errors.Select(x=> x.Code = "404") != null)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Incorrect credentials", 404));
            }
            else
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("something went wrong", 400));

            }
        }
    }
}
