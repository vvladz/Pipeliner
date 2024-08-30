namespace Pipeliner.Sql;

public sealed record SqlLink(string ConnectionString, FormattableString Query)
{
    public SqlLink With(FormattableString query) =>
        this with { Query = query };

    public static SqlLink From(string connectionString) =>
        new(connectionString, null!);
}