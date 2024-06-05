using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;
using Uploader_Api.Dtos;
using Uploader_Api.Models;
using Uploader_Api.Services.Contract;

namespace Uploader_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _PostService;

        public PostController(IPostService postService) { 
           _PostService = postService;
        }
        [HttpGet]
        [Route("getFile")]
        //[Authorize]
        public async Task<IActionResult> GetFile()
        {
            var results = await _PostService.GetFile();
            if(results != null)
            {
                return Ok(results);
            }
            else
            {
                return BadRequest("something went wrong");
            }
        }

        [HttpPost]
        [Route("postFile")]
        [Authorize]
        public async Task<IActionResult> PostFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Bad request", 400));
            }
            try
            {

                var results = await _PostService.SetFile(_PostService.ReadFile(file));
                if (results)
                {
                    return Ok(ApiResponse<bool>.SuccessResponse(true, "sheets uploaded succesfully"));
                }
                else
                {
                    return BadRequest(ApiResponse<bool>.ErrorResponse("something went wrong", 500));

                }
            }
            catch (Exception ex)
            {
                //return BadRequest(ApiResponse<bool>.ErrorResponse("server wg"));
                return BadRequest(ex.Message);

            }

          
        }
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteFile([FromQuery] string UserId)
        {
            if (UserId == null || UserId.Length == 0)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Bad request", 400));
            }
            try
            {

                var results = await _PostService.DeleteFile(UserId);
                if (results)
                {
                    return Ok(ApiResponse<bool>.SuccessResponse(true, "Deleted successfully"));
                }
                else
                {
                    return BadRequest(ApiResponse<bool>.ErrorResponse("something went wrong", 500));

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
