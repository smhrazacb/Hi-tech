using System.ComponentModel.DataAnnotations;

namespace EsparkIndent.Server.Entities
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
