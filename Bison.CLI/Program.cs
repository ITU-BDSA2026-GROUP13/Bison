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
    static string pathToCheepCsvFile = "bison_observe_cli_db.csv";
    static string pathToCommentCSVFile = "bison_comments_cli_db.csv";

    static void Main(string[] args)
    {
        CSVDatabase<Cheep> cheepDatabase = new CSVDatabase<Cheep>(pathToCheepCsvFile);
        CSVDatabase<Comment> commentDatabase = new CSVDatabase<Comment>(pathToCommentCSVFile);
        RootCommand rootCommand = new RootCommand("Application to alter data in database");
        
        var readCommand = new Command("read", "Reads all values from DB");
        readCommand.SetAction((parseResult) =>
        {
            var records = cheepDatabase.Read();
            UserInterface.PrintCheeps(records);
        });
        
        var observeCommand = new Command("observe", "Adds observation to DB");
        var observeArgument = new Argument<string>("observation");
        observeCommand.Add(observeArgument);
        observeCommand.SetAction((parseResult) =>
        {
            string observation = parseResult.GetValue(observeArgument) ?? throw new InvalidOperationException("Message argument was not provided.");
            
            Bison.Cheep record = new Bison.Cheep(observation);
            cheepDatabase.Store(record);
        });
        
        var commentCommand = new Command("comment", "Adds comment to observation");
        var commentArgument = new Argument<string>("comment");
        var commentCheepIDArgument = new Argument<long>("cheepID");
        commentCommand.Add(commentArgument);
        commentCommand.Add(commentCheepIDArgument);
        commentCommand.SetAction((parseResult) =>
        {
            string comment = parseResult.GetValue(commentArgument) ?? throw new InvalidOperationException("Comment argument was not provided.");
            long CheepID = parseResult.GetValue(commentCheepIDArgument);
            
            Bison.Comment commentRecord = new Bison.Comment(CheepID, comment);
            commentDatabase.Store(commentRecord);
        });

        rootCommand.Add(readCommand);
        rootCommand.Add(observeCommand);
        rootCommand.Add(commentCommand);
        
        rootCommand.Parse(args).Invoke(); // Actually takes
    }
}
