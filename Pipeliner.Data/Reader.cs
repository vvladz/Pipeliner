using System.Collections.Concurrent;

namespace Pipeliner.Data;

public abstract class Reader<T> : IReader<T> where T : class
{
    public abstract long Count();

    public virtual IEnumerable<T> Read()
    {
        T? after = null;
        while (Read(after, Const.BatchSize) is [.., var last] batch)
        {
            after = last;
            foreach (var item in batch)
                yield return item;
        }
    }

    public virtual IEnumerable<T> ReadAsync()
    {
        var queue = new BlockingCollection<T>(Const.QueueSize);
        var task = Task.Factory.StartNew(() => AsyncReader(queue), TaskCreationOptions.LongRunning);

        foreach (var item in queue.GetConsumingEnumerable())
            yield return item;

        task.Wait();
    }

    protected abstract T[] Read(T? after, int count);

    private void AsyncReader(BlockingCollection<T> queue)
    {
        try
        {
            foreach (var item in Read())
                queue.Add(item);
            queue.CompleteAdding();
        }
        catch
        {
            queue.CompleteAdding();
            throw;
        }
    }
}