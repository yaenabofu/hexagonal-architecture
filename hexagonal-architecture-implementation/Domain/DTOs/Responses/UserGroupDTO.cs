using Domain.Enums;
using Domain.Models;

namespace Domain.DTOs.Responses
{
    public class UserGroupDTO : BaseIdEntity
    {
        public Group Code { get; set; }
        public string Description { get; set; }
    }
}
