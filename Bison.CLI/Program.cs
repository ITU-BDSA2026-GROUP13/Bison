using System;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using SimpleDB;


public class Program
{
    static string pathToCsvFile = "bison_observe_cli_db.csv";
    
    public record Cheep(string Author, string Observation, long Timestamp);

    static void Main(string[] args)
    {
        CSVDatabase<Cheep> csvDatabase = new CSVDatabase<Cheep>(pathToCsvFile);
        try
        {
            if (args[0] == "read")
            {
                var records = csvDatabase.Read();
                UserInterface.PrintCheeps(records);
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
}
