namespace Pipeliner;

public sealed class Source<TContext, TOut> : Pipeline<TContext, TOut>
{
    private readonly TContext _context;
    private readonly Func<TContext, IEnumerable<TOut>> _source;
    private readonly Func<TContext, long>? _count;
    private readonly string? _name;

    internal Source(
        TContext context,
        Func<TContext, IEnumerable<TOut>> source,
        Func<TContext, long>? count,
        string? name)
        : base(context)
    {
        _context = context;
        _source = source;
        _count = count;
        _name = name;
    }

    internal override IEnumerable<TOut> Run()
    {
        var total = _count?.Invoke(_context) ?? 0;
        var start = DateTime.Now;
        var delay = DateTime.Now;
        var count = 0L;

        foreach (var item in _source(_context))
        {
            ++count;
            yield return item;
            delay = Report(_name, start, delay, count, total, false);
        }

        Report(_name, start, delay, count, total, true);
        Console.WriteLine();
    }

    private static DateTime Report(string? name, DateTime start, DateTime delay, long count, long total, bool end)
    {
        if (name == null)
            return DateTime.Now;

        if (!end && (DateTime.Now - delay).TotalSeconds < 3)
            return delay;

        string Eta() => end ? string.Empty : $"  ETA: {GetEta(start, count, total):hh\\:mm}";

        string Count() => $"[{string.Format($"{{0,{total.ToString().Length}}}", count)} / {total}]";
        
        Console.Write(
                total < 1
                    ? $"\r{name} ({DateTime.Now - start:hh\\:mm}): {count} "
                    : $"\r{name} ({DateTime.Now - start:hh\\:mm}): {(count * 100) / total,3}% {Count()}{Eta()} ");
        
        return DateTime.Now;
    }

    private static TimeSpan GetEta(DateTime start, long count, long total) =>
        TimeSpan.FromTicks((DateTime.Now - start).Ticks * (total - count) / count);
}