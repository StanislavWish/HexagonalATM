using Application.Core.Operations;
using Application.Core.Users;
using Application.Infrastructure;
using Application.Ports;
using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Npgsql;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace PostgresInfrastructure.Repositories;

public class OperationsRepository : IOperationRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public OperationsRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public Collection<OperationInfo> GetByUser(User account)
    {
        ConfiguredTaskAwaitable<NpgsqlConnection> connectionAwaiter = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .ConfigureAwait(false);
        NpgsqlConnection connection = connectionAwaiter.GetAwaiter().GetResult();

        const string sql =
            """
                    SELECT * FROM operations
                    WHERE accountId = :UserAccountId;
            """;
        using var command = new NpgsqlCommand(sql, connection);

        command.AddParameter("UserAccountId", account.Id);

        using NpgsqlDataReader reader = command.ExecuteReader();

        if (reader.Read() == false)
        {
            return new Collection<OperationInfo>();
        }

        var operations = new Collection<OperationInfo>();
        while (reader.Read())
        {
            operations.Add(
                new OperationInfo(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetDecimal(2),
                    (OperationType)Enum.Parse(typeof(OperationType), reader.GetString(3), true)));
        }

        return operations;
    }

    public IEnumerable<OperationInfo> GetAllOperations()
    {
        ConfiguredTaskAwaitable<NpgsqlConnection> connectionAwaiter = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .ConfigureAwait(false);
        NpgsqlConnection connection = connectionAwaiter.GetAwaiter().GetResult();

        const string sql = """ SELECT * FROM operations;""";

        using var command = new NpgsqlCommand(sql, connection);

        using NpgsqlDataReader reader = command.ExecuteReader();

        if (reader.Read() == false)
        {
            return new Collection<OperationInfo>();
        }

        var operations = new Collection<OperationInfo>();
        while (reader.Read())
        {
            operations.Add(
                new OperationInfo(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetDecimal(2),
                    (OperationType)Enum.Parse(typeof(OperationType), reader.GetString(3), true)));
        }

        return operations;
    }

    public void AddOperation(OperationInfo operation)
    {
        const string sql =
            """
            INSERT INTO operations(AccountId, Amount, OperationType) 
            VALUES( @accountId, @Amount, @OperationType);
            """;
        ConfiguredTaskAwaitable<NpgsqlConnection> connectionAwaiter = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .ConfigureAwait(false);
        NpgsqlConnection connection = connectionAwaiter.GetAwaiter().GetResult();
        using var command = new NpgsqlCommand(sql, connection);

        command
            .AddParameter("accountId", operation.UserId)
            .AddParameter("Amount", operation.Amount)
            .AddParameter("OperationType", (int)operation.OperationType);

        command.ExecuteNonQuery();
    }
}