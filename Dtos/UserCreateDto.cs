using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(30)]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        [MinLength(6)]
        public string Password { get; set; }
    }
}