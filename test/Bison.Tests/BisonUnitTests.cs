namespace Bison.Tests;

using Bison;
using Service;
using SimpleDB;

public class BisonUnitTests
{
    [Fact]
    public void TestAddCommentStoresCommentForExistingObservation()
    {
        //Arrange
        var comments = new InMemoryTestDatabaseRepository<Comment>();
        var observations = new InMemoryTestDatabaseRepository<Cheep>();
        var service = new CommentService(comments, observations);
        observations.Store(new Cheep(69, "Lars", "test", "Slagelse", 1000));

        //Act
        service.addComment(new Comment(69, "comment"));
        List<Comment> storedComments = comments.Read().ToList();

        //Assert
        Assert.Single(storedComments);
        Assert.Equal(69, storedComments[0].CheepID);
        Assert.Equal("comment", storedComments[0].Message);
    }

    [Fact]
    public void TestAddCommentThrowsForNonExistingObservation()
    {
        //Arrange
        var comments = new InMemoryTestDatabaseRepository<Comment>();
        var observations = new InMemoryTestDatabaseRepository<Cheep>();
        var service = new CommentService(comments, observations);

        //Act
        Assert.Throws<InvalidOperationException>(() =>
            service.addComment(new Comment(69, "comment")));

        //Assert
        Assert.Empty(comments.Read());
    }

    [Fact]
    public void TestGetCommentsOnlyReturnsCommentsForRequestedObservation()
    {
        //Arrange
        var comments = new InMemoryTestDatabaseRepository<Comment>();
        comments.Store(new Comment(69, "comment for 69"));
        comments.Store(new Comment(70, "comment for 70"));
        comments.Store(new Comment(69, "another comment for 69"));
        var observations = new InMemoryTestDatabaseRepository<Cheep>();
        observations.Store(new Cheep(69, "Lars", "test", "Randers", 1000));
        observations.Store(new Cheep(70, "Peter", "tester", "Taastrup", 1000));
        var service = new CommentService(comments, observations);
        

        //Act
        List<Comment> result = service.getComments(69).ToList();

        //Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(69, result[0].CheepID);
        Assert.Equal(69, result[1].CheepID);
    }

    [Fact]
    public void TestObservationReducedConstructor() 
    {
        //Arrange + Act
        Cheep cheep = new Cheep("Jeg så en ko", "Isen");

        //Assert
        Assert.Equal("Jeg så en ko", cheep.Observation);
        Assert.Equal("Isen", cheep.Location);
    }

    [Fact]
    public void TestObservationFullConstructor() {
        //Arrange + Act
        Cheep cheep = new Cheep(67, "Lars", "Jeg så en ko", "Isen", 1000);

        //Assert
        Assert.Equal(67, cheep.CheepID);
        Assert.Equal("Lars", cheep.Author);
        Assert.Equal("Jeg så en ko", cheep.Observation);
        Assert.Equal("Isen", cheep.Location);
        Assert.Equal(1000, cheep.Timestamp);
    }

    [Fact]
    public void TestAuthorMatchingEnvironmentUsername()
    {
        //Arrange
        Cheep cheep = new Cheep("Jeg så en ko", "Isen");

        //Act
        string cheep_author = cheep.Author;

        //Assert
        Assert.Equal(Environment.UserName, cheep_author);
    }

    [Fact]
    public void TestTimeStampMatchesTimeOfConstruction()
    {
        //Arrange
        long time_before = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); 
        
        //Act
        Cheep cheep = new Cheep("Jeg så en ko", "Isen");
        long time_after = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); 

        //Assert
        Assert.InRange(cheep.Timestamp, time_before, time_after);
    }

    [Fact]
    public void TestCommentReducedConstructor() 
    {
        //Arrange + Act
        Comment comment = new Comment(69, "Thats right I saw it too");

        //Assert
        Assert.Equal(69, comment.CheepID);
        Assert.Equal("Thats right I saw it too", comment.Message);
    }

    [Fact]
    public void TestCommentFullConstructor() 
    {
        //Arrange + Act
        Comment comment = new Comment(69, "Lars", "Thats right I saw it too", 1000);

        //Assert
        Assert.Equal(69, comment.CheepID);
        Assert.Equal("Lars", comment.Author);
        Assert.Equal("Thats right I saw it too", comment.Message);
        Assert.Equal(1000, comment.Timestamp);
    }

    [Fact]
    public void TestDoesObservationExistMethodYES() 
    {
        //Arrange
        List<Cheep> cheeps = new List<Cheep>(){new Cheep(69, "Lars", "test", "Slagelse", 1000)};
        Comment comment = new Comment(69, "Thats right I saw it too");

        //Act
        bool result = CommentHandling.doesObservationExist(comment.CheepID, cheeps);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void TestDoesObservationExistMethodNO()
    {
        //Arrange
        List<Cheep> cheeps = new List<Cheep>(){new Cheep(69, "Lars", "test", "Slagelse", 1000)};
        Comment comment = new Comment(67, "Thats right I saw it too");

        //Act
        bool result = CommentHandling.doesObservationExist(comment.CheepID, cheeps);

        //Assert
        Assert.False(result);
    }


    private sealed class InMemoryTestDatabaseRepository<T> : IDatabaseRepository<T>
    {
        private readonly List<T> records = [];

        public IEnumerable<T> Read(int? limit = null)
        {
            IEnumerable<T> result = records;
            if (limit.HasValue) result = result.Take(limit.Value);

            return result.ToList();
        }

        public void Store(T record)
        {
            records.Add(record);
        }
    }
}