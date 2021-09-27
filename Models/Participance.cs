using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class Participance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string MeetingId { get; set; }
    }
}