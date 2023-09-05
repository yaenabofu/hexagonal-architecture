using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.Driving
{
    public interface IUserService
    {
        public Task<User> AddUser(string login, string password, Group group);
        public Task<User> DeleteUserById(Guid userId);
        public Task<User> DeleteUserByLogin(string login);
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<User> GetUserById(Guid userId);
        public Task<User> GetUserByLogin(string login);
    }
}
