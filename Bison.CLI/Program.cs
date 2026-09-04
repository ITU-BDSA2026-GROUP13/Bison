using System;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using SimpleDB;

class Program
{
    static string pathToCsvFile = "bison_observe_cli_db.csv";

    static void Main(string[] args)
    {
        CSVDatabase<Cheep> csvDatabase = new CSVDatabase<Cheep>(pathToCsvFile);
        try
        {
            if (args[0] == "read")
            {
                var records = csvDatabase.Read();
                foreach (var record in records)
                {
                    var time = DateTimeOffset.FromUnixTimeSeconds(record.Timestamp).ToLocalTime();
                    Console.WriteLine($"{record.Author} @ {time:MM/dd/yy HH:mm:ss:} {record.Observation}");
                }
            }
            else if (args[0] == "observe")
            {
                string author = Environment.UserName;
                long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                Cheep record = new Cheep(author, args[1], timestamp);
                csvDatabase.Store(record);
            }
            else
            {
                Console.WriteLine("Not a supported command");
            }
        }
        catch (IndexOutOfRangeException exc)
        {
            Console.WriteLine($"You passed zero arguments: {exc}");
        }
    }

    public record Cheep(string Author, string Observation, long Timestamp);

    /*public static void read()
    {
        using (StreamReader reader = new StreamReader(pathToCsvFile))
        using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<Cheep>();

            foreach (var record in records)
            {
                var time = DateTimeOffset.FromUnixTimeSeconds(record.Timestamp).ToLocalTime();
                Console.WriteLine($"{record.Author} @ {time:MM/dd/yy HH:mm:ss:} {record.Observation}");
            }
        }
    } 
    
    public static void observe(string observation)
    {
        using (var writer = new StreamWriter(pathToCsvFile, append: true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            string author = Environment.UserName;
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            csv.WriteField(author);
            csv.WriteField(observation, true);
            csv.WriteField(timestamp);
            csv.NextRecord();
        }
    }*/
}
