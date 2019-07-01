using System.ComponentModel.DataAnnotations;

namespace Tiktack.Messaging.WebApi.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
