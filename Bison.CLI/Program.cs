namespace Bison;

using System;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using System.CommandLine;
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
        RootCommand rootCommand = new RootCommand("Application to alter data in database");
        
        var readCommand = new Command("read", "Reads all values from DB");
        readCommand.SetAction((parseResult) =>
        {
            var records = csvDatabase.Read();
            UserInterface.PrintCheeps(records);
        });
        
        var observeCommand = new Command("observe", "Adds observation to DB");
        var observeArgument = new Argument<string>("observation");
        observeCommand.Add(observeArgument);
        observeCommand.SetAction((parseResult) =>
        {
            string observation = parseResult.GetValue(observeArgument) ?? throw new InvalidOperationException("Message argument was not provided.");
            string author = Environment.UserName;
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            
            Cheep record = new Cheep(author, observation, timestamp);
            csvDatabase.Store(record);
        });

        rootCommand.Add(readCommand);
        rootCommand.Add(observeCommand);
        
        rootCommand.Parse(args).Invoke(); // Actually takes
    }
}
