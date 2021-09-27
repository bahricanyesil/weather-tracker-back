namespace webapi.Dtos
{
    public class UserReadDto
    {
        public string UniqueId { get; set; }

        public string UserName { get; set; }

        public string Country { get; set; }
        public string Email { get; set; }

        public string City { get; set; }

        public string NotificationToken { get; set; }
    }
}