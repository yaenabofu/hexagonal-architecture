using Adapter.MsSqlServer.Contexts;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System.Threading.Channels;

namespace Adapter.MsSqlServer.Tests
{
    public class MsSqlServerAdapterTests
    {
        private readonly UserContext _userContext;
        public MsSqlServerAdapterTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<UserContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString(),
                          c => c.EnableNullChecks(false))
                          .Options;
            _userContext = new UserContext(dbContextOptions);
        }
        [Fact]
        public async Task AddUserAsync_Should_Add_User()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);
            var newUser = new User { Id = Guid.NewGuid(), Login = "Login", PasswordHash = "passwordHash" };

            // Act
            var addedUser = await userRepository.AddUser(newUser);

            // Assert
            Assert.NotNull(addedUser);
            Assert.Equal(newUser.Id, addedUser.Id);

            _userContext.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetAllUsersAsync_Should_Return_All_Users()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);

            // Add some test users to the in-memory database
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Login = "Login_111", PasswordHash = "passwordHash" },
                new User { Id = Guid.NewGuid(), Login = "Login_222", PasswordHash = "passwordHash" },
            };
            _userContext.Users.AddRange(users);
            await _userContext.SaveChangesAsync();

            // Act
            var allUsers = await userRepository.GetAllUsers();

            // Assert
            Assert.Equal(users.Count, allUsers.Count());

            _userContext.Database.EnsureDeleted();
        }
        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenExist()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);
            var newUser_1 = new User { Id = Guid.NewGuid(), Login = "Login_111" };
            var newUser_2 = new User { Id = Guid.NewGuid(), Login = "Login_222" };

            // Act
            await userRepository.AddUser(newUser_1);
            await userRepository.AddUser(newUser_2);

            var foundUserById = await userRepository.GetUserById(newUser_1.Id);

            // Assert
            Assert.NotNull(foundUserById);
            Assert.Equal(newUser_1.Id, foundUserById.Id);

            _userContext.Database.EnsureDeleted();
        }
        [Fact]
        public async Task GetUserById_ShouldReturnNull_WhenNotExist()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);
            var newUser_1 = new User { Id = Guid.NewGuid(), Login = "Login_111" };

            // Act
            await userRepository.AddUser(newUser_1);

            var notExistingUserId = Guid.NewGuid();
            var foundUserById = await userRepository.GetUserById(notExistingUserId);

            // Assert
            Assert.Null(foundUserById);
            Assert.NotEqual(notExistingUserId, newUser_1.Id);

            _userContext.Database.EnsureDeleted();
        }
        [Fact]
        public async Task GetUserByLogin_ShouldReturnUser_WhenExist()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);
            var newUser_1 = new User { Id = Guid.NewGuid(), Login = "Login_111" };
            var newUser_2 = new User { Id = Guid.NewGuid(), Login = "Login_222" };

            // Act
            await userRepository.AddUser(newUser_1);
            await userRepository.AddUser(newUser_2);

            var foundUserByLogin = await userRepository.GetUserById(newUser_1.Id);

            // Assert
            Assert.NotNull(foundUserByLogin);
            Assert.Equal(newUser_1.Login, foundUserByLogin.Login);

            _userContext.Database.EnsureDeleted();
        }
        [Fact]
        public async Task GetUserByLogin_ShouldReturnNull_WhenNotExist()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);
            var newUser_1 = new User { Id = Guid.NewGuid(), Login = "Login_111" };

            // Act
            await userRepository.AddUser(newUser_1);

            string notExistingUserLogin = "notExistingLogin";
            var foundUserByLogin = await userRepository.GetUserByLogin(notExistingUserLogin);

            // Assert
            Assert.Null(foundUserByLogin);
            Assert.NotEqual(notExistingUserLogin, newUser_1.Login);

            _userContext.Database.EnsureDeleted();
        }
        [Fact]
        public async Task GetUserGroupByEnum_ShouldReturnUserGroup_WhenExist()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);
            var newUserGroupAdmin = new UserGroup()
            {
                Id = Guid.NewGuid(),
                Code = Group.Admin,
                Description = "desc_AdminGroup"
            };
            var newUserGroupUser = new UserGroup()
            {
                Id = Guid.NewGuid(),
                Code = Group.User,
                Description = "desc_UserGroup"
            };

            // Act
            await _userContext.UserGroups.AddAsync(newUserGroupUser);
            await _userContext.UserGroups.AddAsync(newUserGroupAdmin);
            await _userContext.SaveChangesAsync();

            var foundUserAdminGroup = await userRepository.GetUserGroup(Group.Admin);

            // Assert
            Assert.NotNull(foundUserAdminGroup);
            Assert.Equal(newUserGroupAdmin.Id, foundUserAdminGroup.Id);
            Assert.NotEqual(foundUserAdminGroup.Id, newUserGroupUser.Id);

            _userContext.Database.EnsureDeleted();
        }
        [Fact]
        public async Task GetUserGroupByEnum_ShouldReturnNull_WhenNotExist()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);
            var newUserGroupAdmin = new UserGroup()
            {
                Id = Guid.NewGuid(),
                Code = Group.Admin,
                Description = "desc_AdminGroup"
            };
            var newUserGroupUser = new UserGroup()
            {
                Id = Guid.NewGuid(),
                Code = Group.User,
                Description = "desc_UserGroup"
            };

            // Act
            await _userContext.UserGroups.AddAsync(newUserGroupUser);
            await _userContext.UserGroups.AddAsync(newUserGroupAdmin);
            await _userContext.SaveChangesAsync();

            Group notExistingGroup = (Group)404;

            var foundUserAdminGroup = await userRepository.GetUserGroup(notExistingGroup);

            // Assert
            Assert.Null(foundUserAdminGroup);
            Assert.NotEqual(notExistingGroup, newUserGroupAdmin.Code);
            Assert.NotEqual(notExistingGroup, newUserGroupUser.Code);

            _userContext.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetUserStateByEnum_ShouldReturnUserState_WhenExist()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);
            var newUserStateActive = new UserState()
            {
                Id = Guid.NewGuid(),
                Code = State.Active,
                Description = "desc_ActiveState"
            };
            var newUserStateBlocked = new UserState()
            {
                Id = Guid.NewGuid(),
                Code = State.Blocked,
                Description = "desc_BlockedState"
            };

            // Act
            await _userContext.UserStates.AddAsync(newUserStateBlocked);
            await _userContext.UserStates.AddAsync(newUserStateActive);
            await _userContext.SaveChangesAsync();

            var foundActiveUserState = await userRepository.GetUserState(State.Active);

            // Assert
            Assert.NotNull(foundActiveUserState);
            Assert.Equal(newUserStateActive.Id, foundActiveUserState.Id);
            Assert.NotEqual(foundActiveUserState.Id, newUserStateBlocked.Id);

            _userContext.Database.EnsureDeleted();
        }
        [Fact]
        public async Task GetUserStateByEnum_ShouldReturnNull_WhenNotExist()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);
            var newUserStateActive = new UserState()
            {
                Id = Guid.NewGuid(),
                Code = State.Active,
                Description = "desc_ActiveState"
            };
            var newUserStateBlocked = new UserState()
            {
                Id = Guid.NewGuid(),
                Code = State.Blocked,
                Description = "desc_BlockedState"
            };

            // Act
            await _userContext.UserStates.AddAsync(newUserStateBlocked);
            await _userContext.UserStates.AddAsync(newUserStateActive);
            await _userContext.SaveChangesAsync();

            var notExistingState = (State)404;

            var foundUserState = await userRepository.GetUserState(notExistingState);

            // Assert
            Assert.Null(foundUserState);
            Assert.NotEqual(notExistingState, newUserStateActive.Code);
            Assert.NotEqual(notExistingState, newUserStateBlocked.Code);

            _userContext.Database.EnsureDeleted();
        }
        [Fact]
        public async Task GetUserAdmin_ShouldReturnTrue_WhenExist()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);
            var userAdmin = new User()
            {
                Id = Guid.NewGuid(),
                Login = "adminLogin",
                UserGroupId = Group.Admin,
            };

            // Act
            await _userContext.Users.AddAsync(userAdmin);
            await _userContext.SaveChangesAsync();

            bool IsUserAdminExist = await userRepository.IsUserAdminExist();

            // Assert
            Assert.True(IsUserAdminExist);

            _userContext.Database.EnsureDeleted();
        }
        [Fact]
        public async Task GetUserAdmin_ShouldReturnFalse_WhenNotExist()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Login = "userLogin",
                UserGroupId = Group.User,
            };

            // Act
            await _userContext.Users.AddAsync(user);
            await _userContext.SaveChangesAsync();

            bool IsUserAdminExist = await userRepository.IsUserAdminExist();

            // Assert
            Assert.False(IsUserAdminExist);

            _userContext.Database.EnsureDeleted();
        }

        [Fact]
        public async Task MarkUserAsBlocked_ShouldReturnUserWithBlockedState()
        {
            // Arrange
            var userRepository = new MsSqlServerAdapter(_userContext);
            var userToBlock = new User()
            {
                Id = Guid.NewGuid(),
                Login = "userLogin",
                UserGroupId = Group.User,
                UserStateId = State.Active
            };

            // Act
            await _userContext.Users.AddAsync(userToBlock);
            await _userContext.SaveChangesAsync();

            var blockedUser = await userRepository.MarkUserAsBlocked(userToBlock);

            // Assert
            Assert.Equal(State.Blocked, blockedUser.UserStateId);

            _userContext.Database.EnsureDeleted();
        }
    }
}