namespace Pipeliner.Csv;

public static class CsvExtensions
{
    public static CsvMapper<T> Map<T>(this CsvLink link) =>
        new(link);
    
    public static CsvFile<T> Open<T>(this CsvLink link) =>
        link.Map<T>().Auto().Open();
}