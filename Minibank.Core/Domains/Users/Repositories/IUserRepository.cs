using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetUser(string id);
        public Task CreateUser(User user);
        public Task<List<User>> GetAllUsers();
        public Task UpdateUser(User user);
        public Task DeleteUser(string id);
        public Task<bool> IsUserExists(string id);
        public Task<bool> IsUserWithLogin(string login);
    }
}