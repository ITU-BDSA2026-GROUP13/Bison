namespace Bison.Tests;

using System;
using Xunit;
using Bison;
using SimpleDB;
using Service;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class BisonTests
{
    private readonly string testFileNameCheep = "cheep_bison_observe_test_cli_db.csv";
    private readonly string testFileNameComment = "comment_bison_observe_test_cli_db.csv";
    private readonly CSVDatabase<Cheep> cheeps;
    private readonly CSVDatabase<Comment> comments;
    private readonly CommentService commentService;


    public BisonTests()
    {
        if (File.Exists(testFileNameCheep)) File.Delete(testFileNameCheep);
        if (File.Exists(testFileNameComment)) File.Delete(testFileNameComment);

        cheeps = new CSVDatabase<Cheep>(testFileNameCheep);
        comments = new CSVDatabase<Comment>(testFileNameComment);

        commentService = new CommentService(comments, cheeps);

        Cheep cheep1 = new Cheep(69, "Lars", "test test", "Slagelse", 1000);
        Cheep cheep2 = new Cheep(70,  "Lasse", "lort", "Slagelse", 2000);
        cheeps.Store(cheep1);
        cheeps.Store(cheep2);
    }


    [Fact]
    public void TestCommentReferenceNonExistingObservation()
    {
        //Snapshot of database BEFORE attempting to add new comment
        List<Comment> database_before_add_attempt = comments.Read().ToList();

        //Test the right exception is thrown (and that it even is thrown)
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
             //Attempt to add new comment referencing invalid observationId
             Comment comment = new Comment(67, "test");
             commentService.addComment(comment);
        });

        //Snapshot of database AFTER attempting to add new comment
        List<Comment> database_after_add_attempt = comments.Read().ToList();

        Assert.Equal(database_before_add_attempt.Count, database_after_add_attempt.Count);
        Assert.Equivalent(database_before_add_attempt, database_after_add_attempt);
    }

    [Fact]
    public void TestCommentReferencesCorrectObservation()
    {
        Comment comment = new Comment(69, "test");
        commentService.addComment(comment);

        List<Comment> comment_database_after_add_attempt = comments.Read().ToList();

        Assert.Single(comment_database_after_add_attempt);
        Assert.Equal(69, comment_database_after_add_attempt[0].CheepID);

        List<Cheep> cheep_database = cheeps.Read().ToList();

        bool foundIt = false;
        foreach (Cheep c in cheep_database)
        {
            if (c.CheepID == comment_database_after_add_attempt[0].CheepID) foundIt = true;
        }

        Assert.True(foundIt);
    }

    [Fact]
    public void TestConversionOfUnixTimestamps()
    {
        long timestamp = 1700000000;

        var time = DateTimeOffset.FromUnixTimeSeconds(timestamp).ToLocalTime();
        string timeString = time.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        // InvariantCulture takes care of Windows and Linux formatting date separators differently.
        
        var expectedTime = new DateTimeOffset(2023, 11, 14, 22, 13, 20, TimeSpan.Zero).ToLocalTime();
        Assert.Equal(expectedTime.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture), timeString);
    }

    [Fact]
    public void TestPrintingOfCheeps()
    {
        string firstCheepTime = DateTimeOffset.FromUnixTimeSeconds(1000)
            .ToLocalTime()
            .ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        string secondCheepTime = DateTimeOffset.FromUnixTimeSeconds(2000)
            .ToLocalTime()
            .ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        string expectedOutput = 
        $"Cheeps:\nLars @ {firstCheepTime}: test test at Slagelse\nLasse @ {secondCheepTime}: lort at Slagelse\n";

        using (StringWriter sw = new StringWriter())
        {
            TextWriter originalOutput = Console.Out;

            Console.SetOut(sw);

            try
            {
                UserInterface.PrintCheeps(cheeps.Read());
                string actualOutput = sw.ToString().ReplaceLineEndings("\n");
                // Normalization around line seperators between Windows and Linux Culture. 
                Assert.Equal(expectedOutput, actualOutput);
            }
            finally
            {
                Console.SetOut(originalOutput);
            }
        }
        
    }
}