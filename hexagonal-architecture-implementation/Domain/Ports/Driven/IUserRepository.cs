using Domain.Enums;
using Domain.Models;

namespace Domain.Ports.Driven
{
    public interface IUserRepository
    {
        public Task<User> AddUser(User user);
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<User> GetUserById(Guid userId);
        public Task<User> GetUserByLogin(string login);
        public Task<UserState> GetUserState(State state);
        public Task<UserGroup> GetUserGroup(Group group);
        public Task<bool> IsUserAdminExist();
        public Task<User> MarkUserAsBlocked(User user);
    }
}
