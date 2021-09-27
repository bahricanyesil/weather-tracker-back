using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos
{
    public class MeetingUpdateDto
    {
        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }
    }
}