namespace Bison.Tests;

using Bison;
using SimpleDB;

public class BisonUnitTests
{
    [Fact]
    public void TestAddCommentStoresCommentForExistingObservation()
    {
        var service = new TestService();
        service.AddObservation(new Cheep(69, "Lars", "test", "Slagelse", 1000));

        service.AddComment(new Comment(69, "comment"));

        List<Comment> storedComments = service.ReadComments(69).ToList();
        Assert.Single(storedComments);
        Assert.Equal(69, storedComments[0].CheepID);
        Assert.Equal("comment", storedComments[0].Message);
    }

    [Fact]
    public void TestAddCommentThrowsForNonExistingObservation()
    {
        var service = new TestService();

        Assert.Throws<InvalidOperationException>(() =>
            service.AddComment(new Comment(69, "comment")));

        Assert.Empty(service.ReadComments(69));
    }


    //Den her service er blot til test og rummer altså både observation/comment logic
    //Ikke helt sikker på om det er sådan her man børe gøre det, men tror det er fint
    //vi kan jo ikke bruge vores reele services da de ville store i vores .csv
    private class TestService
    {
        private readonly List<Comment> comments = [];
        private readonly List<Cheep> observations = [];

        public void AddObservation(Cheep observation)
        {
            observations.Add(observation);
        }

        public void AddComment(Comment comment)
        {
            if (!CommentHandling.doesObservationExist(comment.CheepID, observations))
                throw new InvalidOperationException("Observation id does not exist");

            comments.Add(comment);
        }

        public IEnumerable<Comment> ReadComments(long observationId)
        {
            return comments.Where(comment => comment.CheepID == observationId);
        }
    }
}