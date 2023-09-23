using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class User : BaseIdEntity
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }
        public Group UserGroupEnum { get; set; }
        public UserGroup UserGroup { get; set; }
        public State UserStateEnum { get; set; }
        public UserState UserState { get; set; }
    }
}
