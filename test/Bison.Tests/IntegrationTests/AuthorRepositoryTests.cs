namespace Bison.Tests.IntegrationTests;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Repository;

public class AuthorRepositoryTests : IDisposable
{
    readonly SqliteConnection _connection;
    readonly DbContextOptions<BisonDBContext> _contextOptions;
    readonly TestData _data;
    BisonDBContext CreateContext() => new BisonDBContext(_contextOptions);

    public void Dispose() => _connection.Dispose();

    public AuthorRepositoryTests()
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
    
    [Fact]
    public async Task GetAuthorByIdTest()
    {
        //Arrange
        using var context = CreateContext();
        var authorRepository = new AuthorRepository(context);
        string? authorId = _data.OtherObservation.Author.Id;

        //Act
        var result = await authorRepository.GetAuthorByID(authorId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("AuthorB", result.ToString());
        Assert.Equal(_data.OtherObservation.Author, result);

    }
}