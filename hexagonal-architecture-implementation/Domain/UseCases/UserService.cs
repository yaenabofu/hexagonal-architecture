using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Domain.Ports.Driven;
using Domain.Ports.Driving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UseCases
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> AddUser(string login, string password, Group group)
        {
            var userWithSameLogin = await _userRepository.GetUserByLogin(login);
            if (userWithSameLogin is not null)
            {
                throw new UserLoginAlreadyExistException($"user with login:{login} exist");
            }

            if (group is Group.Admin)
            {
                var adminExist = await _userRepository.IsUserAdminExist();

                if (adminExist)
                {
                    throw new UserAdminAlreadyExistException("user with admin group already exist");
                }
            }

            var userGroup = await _userRepository.GetUserGroup(group);
            if (userGroup is null)
            {
                throw new UserGroupNotFoundException($"userGroup:{group} not found");
            }

            var userState = await _userRepository.GetUserState(State.Active);

            var hashedPassword = _passwordHasher.Hash(password);

            var userToAdd = new User()
            {
                Login = login,
                PasswordHash = hashedPassword,
                CreatedDate = DateTime.Now,
                UserGroupId = (int)userGroup.Code,
                UserStateId = (int)userState.Code
            };

            var addedUser = await _userRepository.AddUser(userToAdd);

            return addedUser;
        }

        public async Task<User> DeleteUserById(Guid userId)
        {
            var userToDelete = await _userRepository.GetUserById(userId);

            if (userToDelete is null)
            {
                throw new UserNotFoundException($"user with id:{userId} not found");
            }

            var deletedUser = await _userRepository.MarkUserAsBlocked(userToDelete);

            return deletedUser;
        }

        public async Task<User> DeleteUserByLogin(string login)
        {
            var userToDelete = await _userRepository.GetUserByLogin(login);

            if (userToDelete is null)
            {
                throw new UserNotFoundException($"user with login:{login} not found");
            }

            var deletedUser = await _userRepository.MarkUserAsBlocked(userToDelete);

            return deletedUser;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var allUsers = await _userRepository.GetAllUsers();

            if (allUsers is null || allUsers.Count() is 0)
            {
                throw new UsersNotFoundException("users not found");
            }

            return allUsers;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            var foundUser = await _userRepository.GetUserById(userId);

            if (foundUser is null)
            {
                throw new UserNotFoundException($"user with id:{userId} not found");
            }

            return foundUser;
        }

        public async Task<User> GetUserByLogin(string login)
        {
            var foundUser = await _userRepository.GetUserByLogin(login);

            if (foundUser is null)
            {
                throw new UserNotFoundException($"user with login:{login} not found");
            }

            return foundUser;
        }
    }
}
