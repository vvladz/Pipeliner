// ReSharper disable ConvertIfStatementToReturnStatement
namespace Pipeliner.Data;

public static class Differ
{
    public static IEnumerable<(TLeft? Left, TRight? Right)> Compare<TLeft, TRight>(
        IEnumerable<TLeft> left,
        IEnumerable<TRight> right,
        Func<TLeft, TRight, int> comparer)
    {
        var differ = new Differ<TLeft, TRight>(left, right, comparer);
        while (differ.Next() is { } difference)
            yield return difference;
    }
}

internal sealed class Differ<TLeft, TRight>
{
    private readonly Stepper<TLeft> _left;
    private readonly Stepper<TRight> _right;
    private readonly Func<TLeft, TRight, int> _comparer;

    internal Differ(IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TRight, int> comparer)
    {
        _comparer = comparer;
        _left = new (left);
        _right = new (right);
    }

    public (TLeft? Left, TRight? Right)? Next()
    {
        while (Equal && Step())
        {
        }

        if (Lesser)
            return GetLeft();
        if (Greater)
            return GetRight();

        if (ValidLeft)
            return GetLeft();
        if (ValidRight)
            return GetRight();

        return null;
    }

    private bool Valid => _left.Valid && _right.Valid;
    private bool ValidLeft => _left.Valid;
    private bool ValidRight => _right.Valid;

    private bool Equal => Valid && _comparer(_left.Item, _right.Item) == 0;
    private bool Greater => Valid && _comparer(_left.Item, _right.Item) > 0;
    private bool Lesser => Valid && _comparer(_left.Item, _right.Item) < 0;

    private bool Step() => (_left.Step(), _right.Step()) == (true, true);

    private (TLeft? Left, TRight? Right)? GetLeft()
    {
        var left = _left.Item;
        _left.Step();
        return (left, default);
    }

    private (TLeft? Left, TRight? Right)? GetRight()
    {
        var right = _right.Item;
        _right.Step();
        return (default, right);
    }

    private sealed class Stepper<T>
    {
        private readonly IEnumerator<T> _source;

        public Stepper(IEnumerable<T> source)
        {
            _source = source.GetEnumerator();
            Step();
        }

        public T Item => _source.Current;

        public bool Valid { get; private set; }

        public bool Step() => Valid = _source.MoveNext();
    }
}