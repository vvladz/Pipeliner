namespace Pipeliner.Data;

public interface IWriter<in T> where T : class
{
    void Write(IEnumerable<T> source);
    void WriteAsync(IEnumerable<T> source);
}