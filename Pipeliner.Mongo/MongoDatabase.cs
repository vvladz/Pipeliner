using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Pipeliner.Mongo;

public sealed class MongoDatabase
{
    private readonly IMongoDatabase _db;

    internal MongoDatabase(IMongoDatabase db) =>
        _db = db;

    public MongoCollection<T> Collection<T>(string collection) where T : class =>
        new(_db.GetCollection<T>(collection));

    static MongoDatabase() =>
        ConventionRegistry.Register(
            "default",
            new ConventionPack { new IgnoreExtraElementsConvention(true) },
            _ => true);
}