using Microsoft.EntityFrameworkCore;
using Pipeliner.Data;

namespace Pipeliner.Sql;

public static class SqlExtension
{
    public static SqlQuery<T> Connect<T>(this SqlLink link) where T : class, IHaveId =>
        new(new SqlContext<T>(link.ConnectionString).Table.FromSql(link.Query));
}