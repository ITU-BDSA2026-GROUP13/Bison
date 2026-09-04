using System;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

class Program
{
    static string pathToCsvFile = "bison_observe_cli_db.csv";

    static void Main(string[] args)
    {
        if (args[0] == "read")
        {
            read();
        }
        else if (args[0] == "observe")
        {
            observe(args[1]);
        }
    }

    public record Cheep(string Author, string Observation, long Timestamp);

    public static void read()
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
        DateTimeOffset localTime = DateTimeOffset.Now;
        long unixTime = localTime.ToUnixTimeSeconds();
        string tidspunkt = unixTime.ToString();

        string author = Environment.UserName;


        using(StreamWriter sw = File.AppendText("bison_observe_cli_db.csv"))
        {
            sw.WriteLine(author + "," + '"' + observation + '"' + "," + tidspunkt);
        }
    }
}
