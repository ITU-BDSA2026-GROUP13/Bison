namespace SimpleDB;

using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    internal string pathToCsvFile;

    public CSVDatabase(string pathToCsvFile)
    {
        this.pathToCsvFile = pathToCsvFile;
    }    

    public IEnumerable<T> Read(int? limit = null)
    {
        using (StreamReader reader = new StreamReader(pathToCsvFile))
        using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<T>();
            return records;
        }
    }

    public void Store(T record)
    {
        using (var writer = new StreamWriter(pathToCsvFile, append: true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            //string author = Environment.UserName;
            //long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            csv.WriteRecord(record);
            csv.NextRecord();
            
            //csv.WriteField(author);
            //csv.WriteField(observation, true);
            //csv.WriteField(timestamp);
            //csv.NextRecord();
        }
    }
}