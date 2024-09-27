using Dapper;
using Hana.Core.Context;
using Hana.Users.Contract;
using Hana.Users.Entities;

namespace Hana.Users.Repositories;

public class UserRepository(DataContext context) : IUserRepository
{
    private readonly DataContext _context = context;

    public async Task<IEnumerable<User>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Users
        """;
        return await connection.QueryAsync<User>(sql);
    }

    public async Task<User> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Users 
            WHERE id = @id
        """;
        var result = await connection.QuerySingleOrDefaultAsync<User>(sql, new { id });
        return result!;
    }

    public async Task<User> GetByPhoneNumber(string phoneNum)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Users 
            WHERE phone_number = @phoneNum
        """;
        var result = await connection.QuerySingleOrDefaultAsync<User>(sql, new { phoneNum });
        return result!;
    }

    public async Task<User> GetByEmail(string email)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Users 
            WHERE email = @email
        """;
        var result = await connection.QuerySingleOrDefaultAsync<User>(sql, new { email });
        return result!;
    }

    public async Task Create(User user)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO Users (guid, name, phone_number, address, email, role, pass_hash)
            VALUES (@Guid, @Name, @PhoneNumber, @Address, @Email, @Role, @PasswordHash)
        """;
        await connection.ExecuteAsync(sql, user);
    }

    public async Task Update(User user)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            UPDATE Users 
            SET name = @Name,
                phone_number = @PhoneNumber,
                address = @Address, 
                email = @Email, 
                role = @Role, 
                pass_hash = @PasswordHash
            WHERE id = @Id
        """;
        await connection.ExecuteAsync(sql, user);
    }

    public async Task Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM Users 
            WHERE id = @id
        """;
        await connection.ExecuteAsync(sql, new { id });
    }
}
