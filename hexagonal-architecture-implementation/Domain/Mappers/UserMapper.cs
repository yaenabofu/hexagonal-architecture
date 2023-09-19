
using Domain.DTOs.Responses;
using Domain.Models;

namespace Domain.Mappers
{
    public class UserMapper : IUserMapper
    {
        public UserDTO MapToUserDTO(User user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                Login = user.Login,
                CreatedData = user.CreatedData,
                UserGroup = new UserGroupDTO()
                {
                    Code = user.UserGroupId,
                },
                UserState = new UserStateDTO()
                {
                    Code = user.UserStateId
                }
            };
        }
    }
}
