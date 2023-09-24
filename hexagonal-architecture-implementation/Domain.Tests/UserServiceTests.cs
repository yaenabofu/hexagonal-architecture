using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Domain.Ports.Driven;
using Domain.UseCases;
using Moq;

namespace Domain.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IPasswordHasher> _passwordHasher = new Mock<IPasswordHasher>();
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();

        private readonly UserService _sut;
        public UserServiceTests()
        {
            _sut = new UserService(_userRepository.Object, _passwordHasher.Object);
        }

        [Fact]
        public async void AddNewUserWithAdminGroup_WhenLoginNotExistAndAdminNotExist_ShouldReturnUser()
        {
            string login = "login";
            string password = "password";
            Group userGroup = Group.Admin;
            State userState = State.Active;

            Guid userId = Guid.NewGuid();
            Guid userGroupId = Guid.NewGuid();
            Guid userStateId = Guid.NewGuid();

            _userRepository.Setup(c => c.GetUserByLogin(login)).ReturnsAsync((User)null);
            _userRepository.Setup(c => c.IsUserAdminExist()).ReturnsAsync(false);
            _userRepository.Setup(c => c.GetUserGroup(userGroup)).ReturnsAsync(new UserGroup()
            {
                Id = (int)userGroup,
                Code = userGroup
            });

            _userRepository.Setup(c => c.GetUserState(userState)).ReturnsAsync(new UserState()
            {
                Id = (int)userState,
                Code = userState
            });

            string hashedPassword = "HASHED_PASSWORD";

            var expectedAddedUser = new User()
            {
                Id = userId,
                Login = login,
                PasswordHash = hashedPassword,
                UserGroupId = (int)userGroup,
                UserStateId = (int)userState
            };

            _passwordHasher.Setup(c => c.Hash(password)).Returns(hashedPassword);

            _userRepository.Setup(c => c.AddUser(It.IsAny<User>())).ReturnsAsync(expectedAddedUser);

            var result = await _sut.AddUser(login, password, userGroup);

            Assert.Equal(expectedAddedUser, result);
        }

        [Fact]
        public async Task AddNewUser_WhenLoginExist_ShouldReturnException()
        {
            string login = "login";
            string password = "password";

            _userRepository.Setup(c => c.GetUserByLogin(login)).ReturnsAsync(new User()
            {
                Login = login
            });

            var result = await Assert.ThrowsAsync<UserLoginAlreadyExistException>(async () => await _sut.AddUser(login, password, Group.User));

            var expectedException = new UserLoginAlreadyExistException($"user with login:{login} exist");

            Assert.Equal(expectedException.GetType(), result.GetType());
            Assert.Equal(expectedException.Message, result.Message);
        }

        [Fact]
        public async Task AddNewUserWithAdminGroup_WhenAdminExist_ShouldReturnException()
        {
            string login = "login";
            string password = "password";

            _userRepository.Setup(c => c.GetUserByLogin(login)).ReturnsAsync((User)null);
            _userRepository.Setup(c => c.IsUserAdminExist()).ReturnsAsync(true);

            var result = await Assert.ThrowsAsync<UserAdminAlreadyExistException>(async () => await _sut.AddUser(login, password, Group.Admin));

            var expectedException = new UserAdminAlreadyExistException("user with admin group already exist");

            Assert.Equal(expectedException.GetType(), result.GetType());
            Assert.Equal(expectedException.Message, result.Message);
        }

        [Fact]
        public async Task AddNewUser_WhenGroupNotExist_ShouldReturnException()
        {
            string login = "login";
            string password = "password";
            Group userGroup = (Group)99;

            _userRepository.Setup(c => c.GetUserByLogin(login)).ReturnsAsync((User)null);
            _userRepository.Setup(c => c.IsUserAdminExist()).ReturnsAsync(true);
            _userRepository.Setup(c => c.GetUserGroup(userGroup)).ReturnsAsync((UserGroup)null);

            var result = await Assert.ThrowsAsync<UserGroupNotFoundException>(async () => await _sut.AddUser(login, password, userGroup));

            var expectedException = new UserGroupNotFoundException($"userGroup:{userGroup} not found");

            Assert.Equal(expectedException.GetType(), result.GetType());
            Assert.Equal(expectedException.Message, result.Message);
        }

        [Fact]
        public async Task DeleteUserById_WhenUserNotExist_ShouldReturnException()
        {
            Guid id = Guid.NewGuid();

            _userRepository.Setup(c => c.GetUserById(id)).ReturnsAsync((User)null);

            var result = await Assert.ThrowsAsync<UserNotFoundException>(async () => await _sut.DeleteUserById(id));

            var expectedException = new UserNotFoundException($"user with id:{id} not found");

            Assert.Equal(expectedException.GetType(), result.GetType());
            Assert.Equal(expectedException.Message, result.Message);
        }
        [Fact]
        public async Task DeleteUserById_WhenUserExist_ShouldReturnUser()
        {
            State blockedStateId = State.Blocked;
            State notBlockedStateId = State.Active;

            Guid id = Guid.NewGuid();

            User userToDelete = new User()
            {
                Id = id,
                UserStateId = (int)notBlockedStateId
            };

            User deletedUser = new User()
            {
                Id = id,
                UserStateId = (int)blockedStateId
            };

            _userRepository.Setup(c => c.GetUserById(id)).ReturnsAsync(userToDelete);

            _userRepository.Setup(c => c.MarkUserAsBlocked(userToDelete)).ReturnsAsync(deletedUser);

            var result = await _sut.DeleteUserById(id);

            Assert.Equal(userToDelete.Id, result.Id);
            Assert.NotEqual(userToDelete.UserStateId, result.UserStateId);
            Assert.Equal((int)blockedStateId, result.UserStateId);
        }
        [Fact]
        public async Task DeleteUserByLogin_WhenUserNotExist_ShouldReturnException()
        {
            string login = "login";

            _userRepository.Setup(c => c.GetUserByLogin(login)).ReturnsAsync((User)null);

            var result = await Assert.ThrowsAsync<UserNotFoundException>(async () => await _sut.DeleteUserByLogin(login));

            var expectedException = new UserNotFoundException($"user with login:{login} not found");

            Assert.Equal(expectedException.GetType(), result.GetType());
            Assert.Equal(expectedException.Message, result.Message);
        }
        [Fact]
        public async Task DeleteUserByLogin_WhenUserExist_ShouldReturnUser()
        {
            State blockedStateId = State.Blocked;
            State notBlockedStateId = State.Active;

            string login = "login";

            User userToDelete = new User()
            {
                Login = login,
                UserStateId = (int)notBlockedStateId
            };

            User deletedUser = new User()
            {
                Login = login,
                UserStateId = (int)blockedStateId
            };

            _userRepository.Setup(c => c.GetUserByLogin(login)).ReturnsAsync(userToDelete);

            _userRepository.Setup(c => c.MarkUserAsBlocked(userToDelete)).ReturnsAsync(deletedUser);

            var result = await _sut.DeleteUserByLogin(login);

            Assert.Equal(userToDelete.Login, result.Login);
            Assert.NotEqual(userToDelete.UserStateId, result.UserStateId);
            Assert.Equal((int)blockedStateId, result.UserStateId);
        }

        [Fact]
        public async Task GetUserByLogin_WhenUserNotExist_ShouldReturnException()
        {
            string login = "login";

            _userRepository.Setup(c => c.GetUserByLogin(login)).ReturnsAsync((User)null);

            var result = await Assert.ThrowsAsync<UserNotFoundException>(async () => await _sut.GetUserByLogin(login));

            var expectedException = new UserNotFoundException($"user with login:{login} not found");

            Assert.Equal(expectedException.GetType(), result.GetType());
            Assert.Equal(expectedException.Message, result.Message);
        }
        [Fact]
        public async Task GetUserByLogin_WhenUserExist_ShouldReturnUser()
        {
            User expectedUser = new User()
            {
                Login = "login"
            };

            _userRepository.Setup(c => c.GetUserByLogin(expectedUser.Login)).ReturnsAsync(new User()
            {
                Login = expectedUser.Login
            });

            var foundUser = await _sut.GetUserByLogin(expectedUser.Login);

            Assert.Equal(expectedUser.Login, foundUser.Login);
        }
        [Fact]
        public async Task GetUserById_WhenUserNotExist_ShouldReturnException()
        {
            Guid id = Guid.NewGuid();

            _userRepository.Setup(c => c.GetUserById(id)).ReturnsAsync((User)null);

            var result = await Assert.ThrowsAsync<UserNotFoundException>(async () => await _sut.GetUserById(id));

            var expectedException = new UserNotFoundException($"user with id:{id} not found");

            Assert.Equal(expectedException.GetType(), result.GetType());
            Assert.Equal(expectedException.Message, result.Message);
        }
        [Fact]
        public async Task GetUserById_WhenUserExist_ShouldReturnUser()
        {
            User expectedUser = new User()
            {
                Id = Guid.NewGuid()
            };

            _userRepository.Setup(c => c.GetUserById(expectedUser.Id)).ReturnsAsync(new User()
            {
                Id = expectedUser.Id
            });

            var foundUser = await _sut.GetUserById(expectedUser.Id);

            Assert.Equal(expectedUser.Id, foundUser.Id);
        }
        [Fact]
        public async Task GetAllUsers_WhenUsersNotExist_ShouldReturnException()
        {
            _userRepository.Setup(c => c.GetAllUsers()).ReturnsAsync((List<User>)null);

            var result = await Assert.ThrowsAsync<UsersNotFoundException>(async () => await _sut.GetAllUsers());

            var expectedException = new UsersNotFoundException($"users not found");

            Assert.Equal(expectedException.GetType(), result.GetType());
            Assert.Equal(expectedException.Message, result.Message);
        }
        [Fact]
        public async Task GetAllUsers_WhenUsersExist_ShouldReturnAllUsers()
        {
            var expected = new List<User>()
            {
                new User()
                {
                    Login = "1"
                },
                new User()
                {
                    Login = "2"
                }
            };

            _userRepository.Setup(c => c.GetAllUsers()).ReturnsAsync(expected);

            var foundUsers = await _sut.GetAllUsers();

            Assert.Equal(expected, foundUsers);
        }
    }
}