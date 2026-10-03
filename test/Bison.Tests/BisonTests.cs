namespace Bison.Tests;

// These test can be adapted to the the structure (TO BE CONTINUED!)
/*
using System;
using Xunit;
using Bison;
using SimpleDB;
using Service;

public class BisonTests
{
    private readonly string testFileNameCheep = "cheep_bison_observe_test_cli_db.csv";
    private readonly string testFileNameComment = "comment_bison_observe_test_cli_db.csv";
    private readonly string testFileNameProposal = "proposal_bison_test_cli_db.csv";
    private readonly CSVDatabase<Cheep> cheeps;
    private readonly CSVDatabase<Comment> comments;
    private readonly CSVDatabase<Proposal> proposalDatabase;
    private readonly CommentService commentService;
    private readonly ProposalService proposalService;

    public BisonTests()
    {
        if (File.Exists(testFileNameCheep)) File.Delete(testFileNameCheep);
        if (File.Exists(testFileNameComment)) File.Delete(testFileNameComment);
        if (File.Exists(testFileNameProposal)) File.Delete(testFileNameProposal);

        cheeps = new CSVDatabase<Cheep>(testFileNameCheep);
        comments = new CSVDatabase<Comment>(testFileNameComment);
        proposalDatabase = new CSVDatabase<Proposal>(testFileNameProposal);

        commentService = new CommentService(comments, cheeps);
        proposalService = new ProposalService(proposalDatabase, cheeps);

        cheeps.Store(new Cheep(69, "Lars", "test test", "Slagelse", 1000));
        cheeps.Store(new Cheep(70, "Lasse", "lort", "Slagelse", 2000));
    }

    [Fact]
    public void TestCommentReferenceNonExistingObservation()
    {
        AssertInvalidReferenceDoesNotAdd(comments, () =>
            commentService.addComment(new Comment(67, "test")));
    }

    [Fact]
    public void TestProposalReferenceNonExistingObservation()
    {
        AssertInvalidReferenceDoesNotAdd(proposalDatabase, () =>
            proposalService.addProposal(new Proposal(67, "Purpurhejre")));
    }

    [Fact]
    public void TestCommentReferencesCorrectObservation()
    {
        AssertValidReferenceAdds(comments, () =>
            commentService.addComment(new Comment(69, "test")));
    }

    [Fact]
    public void TestProposalReferencesCorrectObservation()
    {
        AssertValidReferenceAdds(proposalDatabase, () =>
            proposalService.addProposal(new Proposal(69, "Purpurhejre")));
    }

    [Fact]
    public void TestEmptyComment()
    {
        Assert.Throws<ArgumentException>(() =>
            commentService.addComment(new Comment(69, "")));
    }

    [Fact]
    public void TestEmptyProposal()
    {
        Assert.Throws<ArgumentException>(() =>
            proposalService.addProposal(new Proposal(69, "")));
    }

    private void AssertInvalidReferenceDoesNotAdd<T>(CSVDatabase<T> database, Action addAttempt)
    {
        List<T> before = database.Read().ToList();

        Assert.Throws<InvalidOperationException>(addAttempt);

        List<T> after = database.Read().ToList();
        Assert.Equal(before.Count, after.Count);
        Assert.Equivalent(before, after);
    }

    private void AssertValidReferenceAdds<T>(CSVDatabase<T> database, Action addAttempt)
        where T : UserAddition
    {
        List<T> before = database.Read().ToList();

        addAttempt();

        List<T> after = database.Read().ToList();
        Assert.NotEqual(before.Count, after.Count);
        Assert.Single(after);
        Assert.Equal(69, after[0].CheepID);

        List<Cheep> observations = cheeps.Read().ToList();
        Assert.Contains(observations, cheep => cheep.CheepID == after[0].CheepID);
    }
}
*/
