using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class User : BaseIdEntity
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedData { get; set; }
        public Group UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
        public State UserStateId { get; set; }
        public UserState UserState { get; set; }
    }
}
