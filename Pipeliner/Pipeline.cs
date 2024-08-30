namespace Pipeliner;

public abstract class Pipeline
{
    public static Context<T> Context<T>(T context) =>
        new(context);
}

public abstract class Pipeline<TContext, T>
{
    private readonly TContext _context;

    internal Pipeline(TContext context) =>
        _context = context;

    internal abstract IEnumerable<T> Run();

    public Pipeline<TContext, T, TNext> Next<TNext>(IPipeline<TContext, T, TNext> function) =>
        new(_context, this, function);

    public void Flush() =>
        _ = Run().Count();

    public void Run(Action<TContext, IEnumerable<T>> collector) =>
        collector(_context, Run());

    public void Run(Action<IEnumerable<T>> collector) =>
        Run((_, source) => collector(source));

    public void Write(Func<TContext, Action<IEnumerable<T>>> selector) =>
        Run((ctx, source) => selector(ctx)(source));
}

public sealed class Pipeline<TContext, TIn, TOut> : Pipeline<TContext, TOut>
{
    private readonly TContext _context;
    private readonly Pipeline<TContext, TIn> _stack;
    private readonly IPipeline<TContext, TIn, TOut> _function;

    internal Pipeline(TContext context, Pipeline<TContext, TIn> stack, IPipeline<TContext, TIn, TOut> function)
        : base(context)
    {
        _context = context;
        _stack = stack;
        _function = function;
    }

    internal override IEnumerable<TOut> Run() =>
        _stack.Run().SelectMany(input => _function.Run(_context, input));
}