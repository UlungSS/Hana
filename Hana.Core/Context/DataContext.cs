using Npgsql;
using Dapper;
using System.Data;
using Microsoft.Extensions.Options;

using Hana.Core.Models;

namespace Hana.Core.Context;

public class DataContext(IOptions<PersistenceSetting> options) 
{
    private readonly PersistenceSetting _option = options.Value;

    public IDbConnection CreateConnection()
    {
        var connectionString = $"Host={_option.Server}; Database={_option.Database}; Username={_option.UserId}; Password={_option.Password};";
        return new NpgsqlConnection(connectionString);
    }

    public async Task Init()
    {
        await InitDatabase();
        await InitTables();
    }

    private async Task InitDatabase()
    {
        // create database if it doesn't exist
        var connectionString = $"Host={_option.Server}; Database=postgres; Username={_option.UserId}; Password={_option.Password};";
        using var connection = new NpgsqlConnection(connectionString);
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_option.Database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE \"{_option.Database}\"";
            await connection.ExecuteAsync(sql);
        }
    }

    private async Task InitTables()
    {
        // create tables if they don't exist
        using var connection = CreateConnection();
        await _initUsers();

        async Task _initUsers()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS Users (
                    id SERIAL PRIMARY KEY,
                    guid VARCHAR,
                    name VARCHAR,
                    phone_number VARCHAR,
                    email VARCHAR,
                    address VARCHAR,
                    role INTEGER,
                    pass_hash VARCHAR
                );
            """;
            await connection.ExecuteAsync(sql);
        }
    }


}
