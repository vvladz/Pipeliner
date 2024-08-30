using System.Collections.Concurrent;

namespace Pipeliner.Data;

public abstract class Writer<T> : IWriter<T> where T : class
{
    public void Write(IEnumerable<T> source)
    {
        foreach (var batch in source.Chunk(Const.BatchSize))
            Write(batch);
    }

    public void WriteAsync(IEnumerable<T> source)
    {
        var queue = new BlockingCollection<T>(Const.QueueSize);
        var task = Task.Factory.StartNew(() => AsyncWriter(queue), TaskCreationOptions.LongRunning);

        foreach (var item in source.TakeWhile(_ => !queue.IsAddingCompleted))
            queue.Add(item);
        queue.CompleteAdding();

        task.Wait();
    }

    protected abstract void Write(T[] batch);

    private void AsyncWriter(BlockingCollection<T> queue)
    {
        try
        {
            Write(queue.GetConsumingEnumerable());
        }
        catch
        {
            queue.CompleteAdding();
            throw;
        }
    }
}