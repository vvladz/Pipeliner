namespace Pipeliner;

public static class Actions
{
    public static Pipeline<TContext, T> Next<TContext, T>(
        this Pipeline<TContext, T> stack,
        Action<T> action) =>
        stack.Next(new Function<TContext, T>((_, input) => action(input)));

    public static Pipeline<TContext, T> Next<TContext, T>(
        this Pipeline<TContext, T> stack,
        Action<TContext, T> function) =>
        stack.Next(new Function<TContext, T>(function));

    private sealed class Function<TContext, T> : IPipeline<TContext, T, T>
    {
        private readonly Action<TContext, T> _action;

        internal Function(Action<TContext, T> action) =>
            _action = action;

        public IEnumerable<T> Run(TContext context, T input)
        {
            _action(context, input);
            return input.ToArray();
        }
    }
}