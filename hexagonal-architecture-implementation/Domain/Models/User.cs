using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class User : BaseIdEntity
    {
        [Key]
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedData { get; set; }
        public Guid UserGroupId { get; set; }
        public Guid UserStateId { get; set; }
    }
}
