using Pipeliner.Data;

namespace Pipeliner.Sql;

public sealed class SqlQuery<T> : Reader<T> where T : class, IHaveId
{
    internal SqlQuery(IQueryable<T> query) =>
        Instance = query;

    public IQueryable<T> Instance { get; }

    public override long Count() =>
        Instance.Count();

    protected override T[] Read(T? after, int count)
    {
        var query = Instance;
        if (after is { Id: var id })
            query = query.Where(it => it.Id > after.Id);
        return query.Take(count).ToArray();
    }
}