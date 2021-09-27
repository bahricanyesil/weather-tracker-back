
using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string SenderId { get; set; }

        [Required]
        [MinLength(3)]
        public string ReceiverId { get; set; }
        public bool isWaiting { get; set; } = true;

        [Required]
        public string UniqueId { get; set; }
    }
}