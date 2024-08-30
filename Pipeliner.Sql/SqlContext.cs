using Microsoft.EntityFrameworkCore;

namespace Pipeliner.Sql;

public sealed class SqlContext<T> : DbContext where T : class
{
    private readonly string _connectionString;

    internal SqlContext(string connectionString) =>
        _connectionString = connectionString;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(_connectionString);

    public DbSet<T> Table { get; set; } = null!;
}
