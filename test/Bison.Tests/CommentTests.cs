namespace Bison.Tests;

using System;
using Xunit;
using Bison;
using SimpleDB;

public class CommentTests
{
    private readonly string testFileName = "bison_observe_test_cli_db.csv";
    private readonly CSVDatabase<Cheep> cheeps;
    private readonly CSVDatabase<Comment> comments;
    public CommentTests()
    {
        if (File.Exists(testFileName))
        {
            File.Delete(testFileName);
        }

        cheeps = new CSVDatabase<Cheep>(testFileName);
        comments = new CSVDatabase<Comment>("bison_observe_test_cli_db.csv");

        Cheep cheep = new Cheep(69, "Lars", "test test", 1000);
        cheeps.Store(cheep);
    }


    [Fact]
    public void TestCommentReferenceNonExistingObservation()
    {
        //Screenshot of database BEFORE attempting to add new comment
        List<Cheep> database_before_add_attempt = cheeps.Read().ToList();

        //Test the right exception is thrown (and that it even is thrown)
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
             //Attempt to add new comment referencing invalid observationId
            Program.addComment(67, "test", cheeps, comments);
        });

        //Screenshot of database AFTER attempting to add new comment
        List<Cheep> database_after_add_attempt = cheeps.Read().ToList();

        Assert.Equal(database_before_add_attempt.Count, database_after_add_attempt.Count);
        Assert.Equivalent(database_before_add_attempt, database_after_add_attempt);
    }
}