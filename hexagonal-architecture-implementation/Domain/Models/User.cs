using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
        public int UserStateId { get; set; }
        public UserState UserState { get; set; }
    }
}
