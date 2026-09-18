namespace Bison.Tests;

using System;
using Xunit;
using Bison;
using SimpleDB;
using System.Globalization;

public class BisonTests
{
    private readonly string testFileNameCheep = "cheep_bison_observe_test_cli_db.csv";
    private readonly string testFileNameComment = "comment_bison_observe_test_cli_db.csv";
    private readonly CSVDatabase<Cheep> cheeps;
    private readonly CSVDatabase<Comment> comments;
    public BisonTests()
    {
        if (File.Exists(testFileNameCheep)) File.Delete(testFileNameCheep);
        if (File.Exists(testFileNameComment)) File.Delete(testFileNameComment);

        cheeps = new CSVDatabase<Cheep>(testFileNameCheep);
        comments = new CSVDatabase<Comment>(testFileNameComment);

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
            Program.addComment(67, "test", cheeps, comments);
        });

        //Snapshot of database AFTER attempting to add new comment
        List<Comment> database_after_add_attempt = comments.Read().ToList();

        Assert.Equal(database_before_add_attempt.Count, database_after_add_attempt.Count);
        Assert.Equivalent(database_before_add_attempt, database_after_add_attempt);
    }

    [Fact]
    public void TestCommentReferencesCorrectObservation()
    {
        Program.addComment(69, "test", cheeps, comments);

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
        
        Assert.Equal("11/14/2023 23:13:20", timeString);
    }

    [Fact]
    public void TestPrintingOfCheeps()
    {
        string expectedOutput = 
        "Cheeps:\nLars @ 01/01/1970 01:16:40: test test\nLasse @ 01/01/1970 01:33:20: lort\n";

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