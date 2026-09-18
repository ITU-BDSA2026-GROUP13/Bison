namespace Bison;

using System;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using System.CommandLine;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using SimpleDB;
using Spectre.Console;
using System.Net.Http.Headers;
using System.Net.Http.Json;

// This is just so I can tag this commit :D

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        // Sets up HTTP connection
        var baseURL = "http://localhost:5229";
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);
        
        RootCommand rootCommand = new RootCommand("Application to alter data in database");
        
        var readCommand = new Command("read", "Reads all values from DB");
        readCommand.SetAction((parseResult) =>
        {
            var records = cheepDatabase.Read(); // Skal ændres til at tage fra webservicen
            UserInterface.PrintCheeps(records);
        });

        var discussionCommand = new Command("discussion", "Read the discussion on a specific observation");
        var discussionCheepIDArgument = new Argument<long>("CheepID");
        discussionCommand.Add(discussionCheepIDArgument);
        discussionCommand.SetAction((parseResult) =>
        {
            long CheepID = parseResult.GetValue(discussionCheepIDArgument);
            var comments = commentDatabase.Read();
            UserInterface.PrintComments(comments, CheepID);
        });
        
        // CLI option for adding a observation
        var observeCommand = new Command("observe", "Adds observation to DB");
        var observeArgument = new Argument<string>("observation");
        var locationObserveCommand = new Argument<string>("Location");
        observeCommand.Add(observeArgument);
        observeCommand.Add(locationObserveCommand);
        observeCommand.SetAction((parseResult) =>
        {
            string observation = parseResult.GetValue(observeArgument) ?? throw new InvalidOperationException("Message argument was not provided.");
            string location = parseResult.GetValue(locationObserveCommand) ?? throw new InvalidOperationException("Location argument was not provided");
            Bison.Cheep record = new Bison.Cheep(observation, location);
            cheepDatabase.Store(record);
            Console.WriteLine("Successfully added observation");
            
        });
        
        var commentCommand = new Command("comment", "Adds comment to observation");
        var commentArgument = new Argument<string>("comment");
        var commentCheepIDArgument = new Argument<long>("cheepID");
        commentCommand.Add(commentArgument);
        commentCommand.Add(commentCheepIDArgument);
        commentCommand.SetAction((parseResult) =>
        {
            string comment = parseResult.GetValue(commentArgument) ?? throw new InvalidOperationException("Comment argument was not provided.");
            long cheepID = parseResult.GetValue(commentCheepIDArgument);
            
            try {
                addComment(cheepID, comment, cheepDatabase, commentDatabase);
            } catch (InvalidOperationException)
            {
                Console.WriteLine("Referenced Observation ID does not exist");
            }
            
        });

        rootCommand.Add(readCommand);
        rootCommand.Add(discussionCommand);
        rootCommand.Add(observeCommand);
        rootCommand.Add(commentCommand);
        
        rootCommand.Parse(args).Invoke(); // Actually takes
        return 0;
    }

    public static void addComment(long cheepId, string comment, IDatabaseRepository<Cheep> cheepDb, IDatabaseRepository<Comment> commentDb)
    {
        if (!CommentHandling.doesObservationExist(cheepId, cheepDb)) throw new InvalidOperationException("Referenced observation does not exist");
            Bison.Comment commentRecord = new Bison.Comment(cheepId, comment);
            commentDb.Store(commentRecord);
    }
}
