namespace Pipeliner;

public sealed class Context<T>
{
    private readonly T _context;

    internal Context(T context) =>
        _context = context;

    public Pipeline<T, TOut> Read<TOut>(Func<T, IEnumerable<TOut>> source, Func<T, long>? count = null) =>
        new Source<T, TOut>(_context, source, count, null);

    public Pipeline<T, TOut> Read<TOut>(string name, Func<T, IEnumerable<TOut>> source, Func<T, long>? count = null) =>
        new Source<T, TOut>(_context, source, count, name);

    public Pipeline<T, TOut> Read<TOut>(string name, Func<T, TOut[]> source) =>
        new Source<T, TOut>(_context, source, _ => source(_).Length, name);
}