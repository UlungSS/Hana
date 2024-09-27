using Hana.Users.Entities;
using Hana.Users.Models;

namespace Hana.Users.Contract;

public interface IUserServices
{
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(int id);
    Task<User> GetByPhoneNumber(string phoneNum);
    Task<User> GetByEmail(string email);
    Task Create(CreateUserRequest model);
    Task Update(int id, UpdateUserRequest model);
    Task Delete(int id);
}
