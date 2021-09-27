using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos
{
    public class UserChangePassDto
    {
        [Required]
        [MaxLength(30)]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        [MinLength(6)]
        public string OldPassword { get; set; }

        [Required]
        [MaxLength(15)]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}