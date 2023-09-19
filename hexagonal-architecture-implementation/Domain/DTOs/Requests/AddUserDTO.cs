using Domain.Enums;

namespace Domain.DTOs.Requests
{
    public class AddUserDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Group Group { get; set; }
    }
}
