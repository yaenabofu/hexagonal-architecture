using Domain.Enums;

namespace Domain.Models
{
    public class UserState : BaseIdEntity
    {
        public State Code { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
