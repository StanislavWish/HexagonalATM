using FluentMigrator;
using Itmo.Dev.Platform.Postgres.Migrations;

namespace PostgresInfrastructure.Migrations;

[Migration(1)]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        create table users
        (
            UserId INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
            Name VARCHAR(50) NOT NULL UNIQUE,
            PinCode CHAR(4) NOT NULL ,
            Balance DECIMAL NOT NULL DEFAULT 0.0 CONSTRAINT moneyPositive CHECK ( Balance >= 0 )
        );

        CREATE TABLE operations
        (
            OperationID INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY ,
            AccountId INT REFERENCES users(UserId) ,
            Amount DECIMAL,
            OperationType INT 
        );
        """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
        """
            DROP TABLE users CASCADE;
            DROP TABLE operations CASCADE;
        """;
}