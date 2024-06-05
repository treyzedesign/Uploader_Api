using Microsoft.AspNetCore.Identity;
using static Uploader_Api.Dtos.Roles;

namespace Uploader_Api.Models
{
    public class User : IdentityUser
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string userEmail { get; set; }

        public RoleType UserRole {get; set;}
    }
}
