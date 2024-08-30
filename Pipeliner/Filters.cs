namespace Pipeliner;

public static class Filters
{
    public static Pipeline<TContext, T> Include<TContext, T>(
        this Pipeline<TContext, T> stack,
        Func<T, bool> function) =>
        stack.Next(new Function<TContext, T>((_, input) => function(input), true));

    public static Pipeline<TContext, T> Include<TContext, T>(
        this Pipeline<TContext, T> stack,
        Func<TContext, T, bool> function) =>
        stack.Next(new Function<TContext, T>(function, true));

    public static Pipeline<TContext, T> Exclude<TContext, T>(
        this Pipeline<TContext, T> stack,
        Func<T, bool> function) =>
        stack.Next(new Function<TContext, T>((_, input) => function(input), false));

    public static Pipeline<TContext, T> Exclude<TContext, T>(
        this Pipeline<TContext, T> stack,
        Func<TContext, T, bool> function) =>
        stack.Next(new Function<TContext, T>(function, false));

    private sealed class Function<TContext, T> : IPipeline<TContext, T, T>
    {
        private readonly Func<TContext, T, bool> _function;
        private readonly bool _condition;

        internal Function(Func<TContext, T, bool> function, bool condition)
        {
            _function = function;
            _condition = condition;
        }

        public IEnumerable<T> Run(TContext context, T input) =>
            _function(context, input) == _condition
                ? input.ToArray()
                : Enumerable.Empty<T>();
    }
}