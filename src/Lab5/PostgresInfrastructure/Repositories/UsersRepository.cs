using Application.Core.Users;
using Application.Ports;
using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Npgsql;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace PostgresInfrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public UsersRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public User? GetByUsername(string username)
    {
        const string sql =
            """
            SELECT * FROM users
            WHERE name = :username;
            """;

        ConfiguredTaskAwaitable<NpgsqlConnection> connectionAwaiter = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .ConfigureAwait(false);
        NpgsqlConnection connection = connectionAwaiter.GetAwaiter().GetResult();

        using var command = new NpgsqlCommand(sql, connection);
        command.AddParameter("username", username);

        using NpgsqlDataReader reader = command.ExecuteReader();
        if (reader.Read() == false)
        {
            return null;
        }

        return new User(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            Convert.ToDecimal(reader.GetFloat(3)));
    }

    public User ChangeUsersMoney(User account, decimal money)
    {
        string sql =
            """
            UPDATE users
            SET Balance = :money
            WHERE UserId = :accountId;
            """;

        ConfiguredTaskAwaitable<NpgsqlConnection> connectionAwaiter = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .ConfigureAwait(false);
        NpgsqlConnection connection = connectionAwaiter.GetAwaiter().GetResult();

        using var command = new NpgsqlCommand(sql, connection);

        command.AddParameter("money", money).AddParameter("accountId", account.Id);
        command.ExecuteNonQuery();
        return account with { Balance = money };
    }

    public IEnumerable<User> GetAllUsers()
    {
        ConfiguredTaskAwaitable<NpgsqlConnection> connectionAwaiter = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .ConfigureAwait(false);
        NpgsqlConnection connection = connectionAwaiter.GetAwaiter().GetResult();

        const string sql = """SELECT * FROM users;""";

        using var command = new NpgsqlCommand(sql, connection);

        using NpgsqlDataReader reader = command.ExecuteReader();

        if (reader.Read() == false)
        {
            return new Collection<User>();
        }

        var usersCollection = new Collection<User>();
        while (reader.Read())
        {
            usersCollection.Add(
                new User(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    Convert.ToDecimal(reader.GetFloat(3))));
        }

        return usersCollection;
    }

    public void AddUser(User user)
    {
        ConfiguredTaskAwaitable<NpgsqlConnection> connectionAwaiter = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .ConfigureAwait(false);
        NpgsqlConnection connection = connectionAwaiter.GetAwaiter().GetResult();

        const string sql =
            """
            INSERT INTO users(Name, pincode, balance)
            VALUES (:username, :pin, :userbalance);
            """;

        using var command = new NpgsqlCommand(sql, connection);
        command
            .AddParameter("username", user.Name)
            .AddParameter("pin", user.PinCode)
            .AddParameter("userbalance", user.Balance);

        command.ExecuteNonQuery();
    }

    public void DeleteUser(string accountId, string accountName)
    {
        ConfiguredTaskAwaitable<NpgsqlConnection> connectionAwaiter = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .ConfigureAwait(false);
        NpgsqlConnection connection = connectionAwaiter.GetAwaiter().GetResult();

        const string sql =
            """
            DELETE FROM Users WHERE UserId = :id and username = :username);
            """;

        using var command = new NpgsqlCommand(sql, connection);
        command.AddParameter("id", accountId)
            .AddParameter("username", accountName);

        command.ExecuteNonQuery();
    }
}