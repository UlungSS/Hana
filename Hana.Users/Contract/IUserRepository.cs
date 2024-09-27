
using Hana.Users.Entities;

namespace Hana.Users.Contract;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(int id);
    Task<User> GetByPhoneNumber(string phoneNum);
    Task<User> GetByEmail(string email);
    Task Create(User user);
    Task Update(User user);
    Task Delete(int id);
}
