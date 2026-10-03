namespace Bison.Tests;

using System.Globalization;

// These test can be adapted to the the structure (TO BE CONTINUED!)

using System;
using Xunit;
using Bison;

public class BisonTests
{
    /*
    private readonly string testFileNameObservation = "observation_bison_observe_test_cli_db.csv";
    private readonly string testFileNameComment = "comment_bison_observe_test_cli_db.csv";
    private readonly string testFileNameProposal = "proposal_bison_test_cli_db.csv";
    private readonly CSVDatabase<Observation> observations;
    private readonly CSVDatabase<Comment> comments;
    private readonly CSVDatabase<Proposal> proposalDatabase;
    private readonly CommentService commentService;
    private readonly ProposalService proposalService;

    public BisonTests()
    {
        if (File.Exists(testFileNameObservation)) File.Delete(testFileNameObservation);
        if (File.Exists(testFileNameComment)) File.Delete(testFileNameComment);
        if (File.Exists(testFileNameProposal)) File.Delete(testFileNameProposal);

        observations = new CSVDatabase<Observation>(testFileNameObservation);
        comments = new CSVDatabase<Comment>(testFileNameComment);
        proposalDatabase = new CSVDatabase<Proposal>(testFileNameProposal);

        commentService = new CommentService(comments, observations);
        proposalService = new ProposalService(proposalDatabase, observations);

        observations.Store(new Observation(69, "Lars", "test test", "Slagelse", 1000));
        observations.Store(new Observation(70, "Lasse", "lort", "Slagelse", 2000));
    }
*/
[Fact]
    public void TestConversionOfUnixTimestamps()
    {
        //Arrange
        long timestamp = 1700000000;

        //Act
        var time = DateTimeOffset.FromUnixTimeSeconds(timestamp).ToLocalTime();
        string timeString = time.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        // InvariantCulture takes care of Windows and Linux formatting date separators differently.
        
        var expectedTime = new DateTimeOffset(2023, 11, 14, 22, 13, 20, TimeSpan.Zero).ToLocalTime();
        
        //Assert
        Assert.Equal(expectedTime.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture), timeString);
    }
    /*

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
        Assert.Equal(69, after[0].ObservationID);

        List<Observation> observations = observations.Read().ToList();
        Assert.Contains(observations, observation => observation.ObservationID == after[0].ObservationID);
    }
    */
}

