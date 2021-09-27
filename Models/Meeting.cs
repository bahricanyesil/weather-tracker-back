using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class Meeting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Country { get; set; }

        [Required]
        [MinLength(3)]
        public string City { get; set; }

        [Required]
        public string UniqueId { get; set; }
    }
}