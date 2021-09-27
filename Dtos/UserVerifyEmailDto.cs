using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos
{
    public class UserVerifyEmailDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(5)]
        public string Code { get; set; }

        [Required]
        public string Token { get; set; }
    }
}