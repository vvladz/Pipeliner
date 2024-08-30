using MongoDB.Driver;

namespace Pipeliner.Mongo;

public static class MongoExtension
{
    public static MongoCollection<T> Connect<T>(this MongoLink link) where T : class =>
        Connect(link).Collection<T>(link.Collection);

    public static MongoDatabase Connect(this MongoLink link) =>
        new(new MongoClient(link.Uri).GetDatabase(link.Db));
}