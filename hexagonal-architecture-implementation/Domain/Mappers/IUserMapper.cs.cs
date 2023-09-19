using Domain.DTOs.Responses;
using Domain.Models;

namespace Domain.Mappers
{
    public interface IUserMapper
    {
        UserDTO MapToUserDTO(User user);
    }
}
