namespace Bison.Tests.IntegrationTests;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Repository;

public class CommentRepositoryTests : IDisposable
{
    // Setup for the In-memory SQLite DB
    readonly SqliteConnection _connection;
    readonly DbContextOptions<BisonDBContext> _contextOptions;
    readonly TestData _data;
    
    BisonDBContext CreateContext() => new BisonDBContext(_contextOptions);
    public void Dispose() => _connection.Dispose();
    
    public CommentRepositoryTests()
    {
        // Open connection to sqlite in-memory DB
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // Apply options to be used for this test suite
        _contextOptions = new DbContextOptionsBuilder<BisonDBContext>()
            .UseSqlite(_connection)
            .Options;

        // Create schema and seed DB
        using var context = new BisonDBContext(_contextOptions);
        context.Database.EnsureCreated();
        _data = TestDbInitializer.Seed(context);
    }
    
    
    // TESTS START HERE:

    [Fact]
    public async Task ReadCommentTest()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new CommentRepository(context);
        long id = _data.Comments.First().PostId;

        // Act
        var result = await repo.ReadComment(id);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(_data.Comments.First().Text, result.First().Text);
    }
    
    [Fact]
    public async Task ReadCommentWithNonExistingIDTest()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new CommentRepository(context);
        long id = 8158000085;

        // Act
        var result = await repo.ReadComment(id);
        
        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateCommentTest()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new CommentRepository(context);
        
        context.Attach(_data.AuthorC); // Attach tells the context that the author and observation already exists in the DB.
        context.Attach(_data.ObservationWithComments);
        
        Comment newComment = new Comment()
        {
            Author = _data.AuthorC,
            Text = "Very Nice Comment",
            TimeStamp = DateTime.Now,
            Observation = _data.ObservationWithComments
        };
        
        // Act
        await repo.CreateComment(newComment);
        var result =  await repo.ReadComment(newComment.PostId);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(newComment.Text, result.First().Text);
    }
    
    [Fact]
    public async Task CreateEmptyCommentTest()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new CommentRepository(context);
        Comment emptyComment = new Comment();
        
        // Act
        var exception = await Assert.ThrowsAsync<DbUpdateException>(async () => await repo.CreateComment(emptyComment));
        
        //Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task GetCommentsByObsIdTest()
    {
        //Arrange
        using var context = CreateContext();
        var repo = new CommentRepository(context);
        long obsId = _data.ObservationWithComments.PostId;
        
        //Act
        var result = await repo.GetCommentsByObsId(obsId);
        
        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal("Nice find!", result.First().Text);
        Assert.Equal(_data.ObservationWithComments.PostId, result.Last().Observation.PostId);
    }
    
    [Fact]
    public async Task GetCommentsByNonExistingObsIdTest()
    {
        //Arrange
        using var context = CreateContext();
        var repo = new CommentRepository(context);
        long fakeObsId = 8158000085;
        
        //Act
        var result = await repo.GetCommentsByObsId(fakeObsId);
        
        //Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
}