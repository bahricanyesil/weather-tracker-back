using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos
{
    public class ParticipanceDeleteDto
    {
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string MeetingId { get; set; }
    }
}