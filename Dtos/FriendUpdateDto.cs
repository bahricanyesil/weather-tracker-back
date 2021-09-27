using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos
{
    public class FriendUpdateDto
    {
        [Required]
        public string SenderId { get; set; }

        [Required]
        public string ReceiverId { get; set; }
    }
}