
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
                CreatedData = user.CreatedDate,
                UserGroup = new UserGroupDTO()
                {
                    Id = user.UserGroup.Id,
                    Code = user.UserGroup.Code,
                    Description = user.UserGroup.Description,
                },
                UserState = new UserStateDTO()
                {
                    Id = user.UserState.Id,
                    Code = user.UserState.Code,
                    Description = user.UserState.Description,
                }
            };
        }
    }
}
