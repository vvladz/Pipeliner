namespace Pipeliner;

public static class Functions
{
    public static Pipeline<TContext, TOut> Next<TContext, TIn, TOut>(
        this Pipeline<TContext, TIn> stack,
        Func<TIn, IEnumerable<TOut>> function) =>
        stack.Next(new Function<TContext, TIn, TOut>((_, input) => function(input)));

    public static Pipeline<TContext, TOut> Next<TContext, TIn, TOut>(
        this Pipeline<TContext, TIn> stack,
        Func<TContext, TIn, IEnumerable<TOut>> function) =>
        stack.Next(new Function<TContext, TIn, TOut>(function));

    public static Pipeline<TContext, TOut> Next<TContext, TIn, TOut>(
        this Pipeline<TContext, TIn> stack,
        Func<TIn, TOut> function) =>
        stack.Next(new Function<TContext, TIn, TOut>((_, input) => function(input).ToArray()));

    public static Pipeline<TContext, TOut> Next<TContext, TIn, TOut>(
        this Pipeline<TContext, TIn> stack,
        Func<TContext, TIn, TOut> function) =>
        stack.Next(new Function<TContext, TIn, TOut>((ctx, input) => function(ctx, input).ToArray()));

    private sealed class Function<TContext, TIn, TOut> : IPipeline<TContext, TIn, TOut>
    {
        private readonly Func<TContext, TIn, IEnumerable<TOut>> _function;

        internal Function(Func<TContext, TIn, IEnumerable<TOut>> function) =>
            _function = function;

        public IEnumerable<TOut> Run(TContext context, TIn input) =>
            _function(context, input);
    }
}