using Domain.Enums;
using Domain.Models;

namespace Domain.DTOs.Responses
{
    public class UserStateDTO : BaseIdEntity
    {
        public State Code { get; set; }
        public string Description { get; set; }
    }
}
