using Microsoft.AspNetCore.Mvc;
using Uploader_Api.Dtos;
using Uploader_Api.Models;

namespace Uploader_Api.Services.Contract
{
    public interface IPostService
    {
        string ReadFile(IFormFile file);
        Task<bool> SetFile(string file);

        Task<IEnumerable<Sheet>> GetFile();

        Task<bool> DeleteFile(string UserId);
    }
}
