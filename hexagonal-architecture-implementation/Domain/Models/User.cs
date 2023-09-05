using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class User : BaseIdEntity
    {
        [Key]
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreatedData { get; set; }
        public string RefreshToken { get; set; }
        public Guid UserGroupId { get; set; }
        public Guid UserStateId { get; set; }
    }
}
