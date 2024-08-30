using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Pipeliner.Data;

namespace Pipeliner.Mongo;

public sealed class MongoCollection<T> : Duplex<T> where T : class
{
    private const string Id = "_id";

    internal MongoCollection(IMongoCollection<T> instance) =>
        Instance = instance;

    public IMongoCollection<T> Instance { get; }

    public IEnumerable<T> Enumerate() =>
        Instance.Find(Builders<T>.Filter.Empty).ToEnumerable();

    public IEnumerable<T> Find(Expression<Func<T, bool>> query) =>
        Instance.Find(query).ToEnumerable();

    public T? One(Expression<Func<T, bool>> query) =>
        Instance.Find(query).FirstOrDefault();

    public bool Any(Expression<Func<T, bool>> query) =>
        Instance.Find(query).Any();

    public void Insert(T item) =>
        Instance.InsertOne(item);

    public override long Count() =>
        Instance.EstimatedDocumentCount();

    protected override T[] Read(T? after, int count) =>
        Instance
            .Find(after == null
                ? Builders<T>.Filter.Empty
                : Builders<T>.Filter.Gt(Id, after.ToBsonDocument()[Id]))
            .Project(CreateProjection())
            .Sort(Builders<T>.Sort.Ascending(Id))
            .Limit(count)
            .ToEnumerable()
            .ToArray();

    protected override void Write(T[] batch)
    {
        try
        {
            Instance.InsertMany(batch);
        }
        catch (MongoException e)
        {
        }
    }

    private static ProjectionDefinition<T, T> CreateProjection()
    {
        var classMap = BsonClassMap.LookupClassMap(typeof(T));
        var projection = Builders<T>.Projection.Include(classMap.AllMemberMaps.First().ElementName);
        return classMap.AllMemberMaps.Skip(1).Aggregate(projection, (p, m) => p.Include(m.ElementName));
    }
}