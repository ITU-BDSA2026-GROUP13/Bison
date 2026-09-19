namespace Bison;

using System;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using System.CommandLine;
using System.Net;
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
        readCommand.SetAction(async (parseResult, ct) =>
        {
            try
            {
                var records = await client.GetFromJsonAsync<List<Cheep>>("observations", ct) ?? new List<Cheep>();
                UserInterface.PrintCheeps(records);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request to web service failed: {ex.Message}");
            }
        });

        var discussionCommand = new Command("discussion", "Read the discussion on a specific observation");
        var discussionCheepIDArgument = new Argument<long>("CheepID");
        discussionCommand.Add(discussionCheepIDArgument);
        discussionCommand.SetAction(async (parseResult, ct) =>
        {
            long cheepID = parseResult.GetValue(discussionCheepIDArgument);
            try
            {
                var comments = await client.GetFromJsonAsync<List<Comment>>($"comments?id={cheepID}", ct) ?? new List<Comment>();
                UserInterface.PrintComments(comments, cheepID);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request to web service failed: {ex.Message}");
            }
        });

        var observeCommand = new Command("observe", "Adds observation to DB");
        var observeArgument = new Argument<string>("observation");
        var locationObserveCommand = new Argument<string>("Location");
        observeCommand.Add(observeArgument);
        observeCommand.Add(locationObserveCommand);
        observeCommand.SetAction(async (parseResult, ct) =>
        {
            string observation = parseResult.GetValue(observeArgument) ?? throw new InvalidOperationException("Message argument was not provided.");
            string location = parseResult.GetValue(locationObserveCommand) ?? throw new InvalidOperationException("Location argument was not provided");
            var record = new Cheep(observation, location);
            try
            {
                var response = await client.PostAsJsonAsync("observation", record, ct);
                Console.WriteLine(response.IsSuccessStatusCode
                    ? "Successfully added observation"
                    : $"Failed: {(int)response.StatusCode} {response.ReasonPhrase}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request to web service failed: {ex.Message}");
            }
        });

        var commentCommand = new Command("comment", "Adds comment to observation");
        var commentArgument = new Argument<string>("comment");
        var commentCheepIDArgument = new Argument<long>("cheepID");
        commentCommand.Add(commentArgument);
        commentCommand.Add(commentCheepIDArgument);
        commentCommand.SetAction(async (parseResult, ct) =>
        {
            string comment = parseResult.GetValue(commentArgument) ?? throw new InvalidOperationException("Comment argument was not provided.");
            long cheepID = parseResult.GetValue(commentCheepIDArgument);
            var record = new Comment(cheepID, comment);
            try
            {
                var response = await client.PostAsJsonAsync("comment", record, ct);
                if (response.StatusCode == HttpStatusCode.NotFound)
                    Console.WriteLine("Referenced Observation ID does not exist");
                else if (!response.IsSuccessStatusCode)
                    Console.WriteLine($"Failed: {(int)response.StatusCode} {response.ReasonPhrase}");
                else
                    Console.WriteLine("Successfully added comment");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request to web service failed: {ex.Message}");
            }
        });

        rootCommand.Add(readCommand);
        rootCommand.Add(discussionCommand);
        rootCommand.Add(observeCommand);
        rootCommand.Add(commentCommand);

        return await rootCommand.Parse(args).InvokeAsync();
    }
}