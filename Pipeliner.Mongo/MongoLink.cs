namespace Pipeliner.Mongo;

public sealed record MongoLink(string Uri, string Db, string Collection)
{
    public MongoLink WithDb(string db) =>
        this with { Db = db };

    public MongoLink WithCollection(string collection) =>
        this with { Collection = collection };

    public static MongoLink From(string uri) =>
        new(uri, null!, null!);

    public static MongoLink Local() =>
        From("mongodb://loclahost:27017");
}