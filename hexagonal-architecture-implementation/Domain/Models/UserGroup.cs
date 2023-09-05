using Domain.Enums;

namespace Domain.Models
{
    public class UserGroup : BaseIdEntity
    {
        public Group Code { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
