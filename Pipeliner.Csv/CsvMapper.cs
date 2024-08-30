using System.Globalization;
using System.Linq.Expressions;
using CsvHelper.Configuration;

namespace Pipeliner.Csv;

public sealed class CsvMapper<T>
{
    private readonly CsvLink _link;
    private readonly ClassMap<T> _classMap;

    internal CsvMapper(CsvLink link)
    {
        _link = link;
        _classMap = new DefaultClassMap<T>();
    }

    public CsvMapper<T> Map<TMember>(Expression<Func<T, TMember>> expression, string column)
    {
        _classMap.Map(expression).Name(column);
        return this;
    }

    public CsvMapper<T> Auto()
    {
        _classMap.AutoMap(CultureInfo.InvariantCulture);
        return this;
    }

    public CsvFile<T> Open()
    {
        _classMap.AutoMap(CultureInfo.InvariantCulture);
        return new CsvFile<T>(_link, _classMap);
    }
}