namespace Pipeliner;

public interface IPipeline<in TContext, in TIn, out TOut>
{
    IEnumerable<TOut> Run(TContext context, TIn input);
}