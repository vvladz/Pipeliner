namespace Pipeliner.Data;

public abstract class Duplex<T> : IReader<T>, IWriter<T> where T : class
{
    private sealed class Reader : Reader<T>
    {
        private readonly Duplex<T> _self;

        internal Reader(Duplex<T> self) =>
            _self = self;

        public override long Count() =>
            _self.Count();

        protected override T[] Read(T? after, int count) =>
            _self.Read(after, count);
    }

    private sealed class Writer : Writer<T>
    {
        private readonly Duplex<T> _self;

        public Writer(Duplex<T> self) =>
            _self = self;

        protected override void Write(T[] batch) =>
            _self.Write(batch);
    }

    private readonly Reader _reader;
    private readonly Writer _writer;

    protected Duplex()
    {
        _reader = new(this);
        _writer = new(this);
    }

    protected abstract T[] Read(T? after, int count);

    protected abstract void Write(T[] batch);

    public abstract long Count();

    public virtual IEnumerable<T> Read() =>
        _reader.Read();

    public virtual IEnumerable<T> ReadAsync() =>
        _reader.ReadAsync();

    public virtual void Write(IEnumerable<T> source) =>
        _writer.Write(source);

    public virtual void WriteAsync(IEnumerable<T> source) =>
        _writer.WriteAsync(source);
}
