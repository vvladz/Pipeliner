namespace Pipeliner.Csv;

public sealed record CsvLink(string Path, string Delimiter, char Quote)
{
    public CsvLink With(string delimiter) =>
        this with { Delimiter = delimiter };

    public CsvLink With(char quote) =>
        this with { Quote = quote };

    public static CsvLink From(string path) =>
        new(path, ",", '"');
}