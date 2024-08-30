using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Pipeliner.Csv;

public sealed class CsvFile<T>
{
    private readonly CsvLink _link;
    private readonly ClassMap<T> _classMap;

    internal CsvFile(CsvLink link, object classMap)
    {
        _link = link;
        _classMap = new DefaultClassMap<T>();
    }

    public long Count() =>
        File.ReadLines(_link.Path).Count() - 1;

    public IEnumerable<T> Read()
    {
        using var reader = new CsvReader(new StreamReader(_link.Path),
            new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = _link.Delimiter,
                Quote = _link.Quote
            });
        reader.Context.RegisterClassMap(_classMap);

        foreach (var record in reader.GetRecords<T>())
            yield return record;
    }

    public void Write(IEnumerable<T> records)
    {
        using var writer = new CsvWriter(new StreamWriter(_link.Path),
            new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = _link.Delimiter,
                Quote = _link.Quote
            });
        writer.Context.RegisterClassMap(_classMap);

        writer.WriteHeader<T>();
        writer.NextRecord();
        foreach (var record in records)
        {
            writer.WriteRecord(record);
            writer.NextRecord();
        }

        writer.Flush();
    }
}