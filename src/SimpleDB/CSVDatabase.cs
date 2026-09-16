namespace SimpleDB;

using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    internal string pathToCsvFile;

    // A constructer that takes the path to an csv file as argument and if it does not exist create e new file at that location
    public CSVDatabase(string pathToCsvFile)
    {
        this.pathToCsvFile = pathToCsvFile;

        bool fileIsEmpty = !File.Exists(this.pathToCsvFile) || new FileInfo(this.pathToCsvFile).Length == 0;

        if (fileIsEmpty)
        {
            using (var writer = new StreamWriter(pathToCsvFile, append: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<T>();
                csv.NextRecord();
            }
        }
    }    

    // Reads all lines from the csv file into a list and returns it
    public IEnumerable<T> Read(int? limit = null)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower().Trim()
        };
        
        using (StreamReader reader = new StreamReader(pathToCsvFile))
        using (CsvReader csv = new CsvReader(reader, config))
        {
            var records = csv.GetRecords<T>().ToList();
            return records;
        }
    }
    
    //Appends a line to the csv file
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