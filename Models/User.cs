using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(40)]
        [MinLength(12)]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        [MinLength(6)]
        public string Password { get; set; }

        [MinLength(3)]
        public string Country { get; set; } = "Turkey";

        [MinLength(3)]
        public string City { get; set; } = "Istanbul";

        [Required]
        public string UniqueId { get; set; }

        [MinLength(3)]
        public string NotificationToken { get; set; } = "";

        public bool IsVerified { get; set; } = false;

    }
}