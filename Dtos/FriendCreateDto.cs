using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos
{
    public class FriendCreateDto
    {
        [Required]
        public string SenderId { get; set; }

        [Required]
        public string ReceiverId { get; set; }
    }
}