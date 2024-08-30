namespace Pipeliner;

public static class Tools
{
    public static T[] ToArray<T>(this T value) =>
        new[] { value };

    public static T[] ToArray<T>(this IEnumerable<T> value) =>
        Enumerable.ToArray(value);

    public static T[] ToArray<T>(this IQueryable<T> value) =>
        Enumerable.ToArray(value);
}