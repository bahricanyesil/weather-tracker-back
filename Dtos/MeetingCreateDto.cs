using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos
{
    public class MeetingCreateDto
    {
        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public IEnumerable<string> participantIds { get; set; }
    }
}