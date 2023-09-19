using Domain.Models;

namespace Domain.DTOs.Responses
{
    public class UserDTO : BaseIdEntity
    {
        public string Login { get; set; }
        public DateTime CreatedData { get; set; }
        public UserGroupDTO UserGroup { get; set; }
        public UserStateDTO UserState { get; set; }
    }
}
