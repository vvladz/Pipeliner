namespace Pipeliner.Data;

public interface IReader<out T> where T : class
{
    long Count();

    IEnumerable<T> Read();
    IEnumerable<T> ReadAsync();
}