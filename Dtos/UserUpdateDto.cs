using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos
{
    public class UserUpdateDto
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string NotificationToken { get; set; }
    }
}