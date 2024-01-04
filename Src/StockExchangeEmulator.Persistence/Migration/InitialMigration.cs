using FluentMigrator;

namespace StockExchangeEmulator.Persistence.Migration;

[Migration(1, "Initial migration")]
public class InitialMigration : FluentMigrator.Migration
{
    public override void Up()
    {
        Create
            .Table("PriceChange")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Value").AsInt16().NotNullable()
            .WithColumn("DateTime").AsDateTime();

        Create
            .Table("Price")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Value").AsInt32().NotNullable()
            .WithColumn("DateTime").AsDateTime();
    }

    public override void Down()
    {
        Delete
            .Table("PriceChange");

        Delete
            .Table("Price");
    }
}
