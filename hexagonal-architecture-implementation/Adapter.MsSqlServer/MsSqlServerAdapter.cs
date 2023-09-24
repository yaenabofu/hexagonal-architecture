using Adapter.MsSqlServer.Contexts;
using Domain.Enums;
using Domain.Models;
using Domain.Ports.Driven;
using Microsoft.EntityFrameworkCore;

namespace Adapter.MsSqlServer
{
    public class MsSqlServerAdapter : IUserRepository
    {
        private readonly UserContext _userContext;

        public MsSqlServerAdapter(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<User> AddUser(User user)
        {
            var addedUser = await _userContext.Users.AddAsync(user);

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }

            return addedUser.Entity;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var allUsers = await _userContext.Users.Include(u => u.UserGroup)
                .Include(u => u.UserState)
                .ToListAsync();

            return allUsers;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            var userWithId = await _userContext.Users.Include(u => u.UserGroup)
                .Include(u => u.UserState)
                .FirstOrDefaultAsync(c => c.Id.Equals(userId));

            return userWithId;
        }

        public async Task<User> GetUserByLogin(string login)
        {
            var userWithLogin = await _userContext.Users.Include(u => u.UserGroup)
                .Include(u => u.UserState)
                .FirstOrDefaultAsync(c => c.Login.Equals(login));

            return userWithLogin;
        }

        public async Task<UserGroup> GetUserGroup(Group group)
        {
            var userGroup = await _userContext.UserGroups.FirstOrDefaultAsync(c => c.Code.Equals(group));

            return userGroup;
        }

        public async Task<UserState> GetUserState(State state)
        {
            var userState = await _userContext.UserStates.FirstOrDefaultAsync(c => c.Code.Equals(state));

            return userState;
        }

        public async Task<bool> IsUserAdminExist()
        {
            var userWithAdminGroup = await _userContext.Users.FirstOrDefaultAsync(c => c.UserGroupId.Equals((int)Group.Admin));

            return userWithAdminGroup is not null;
        }

        public async Task<User> MarkUserAsBlocked(User user)
        {
            var blockedUserState = await GetUserState(State.Blocked);
            user.UserStateId = blockedUserState.Id;
            user.UserState = blockedUserState;

            await _userContext.SaveChangesAsync();

            return user;
        }
    }
}