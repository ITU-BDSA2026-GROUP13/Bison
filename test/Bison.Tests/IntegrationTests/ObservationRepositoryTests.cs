namespace Bison.Tests.IntegrationTests;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Repository;

public class ObservationRepositoryTests : IDisposable
{
    // Setup for the In-memory SQLite DB
    readonly SqliteConnection _connection;
    readonly DbContextOptions<BisonDBContext> _contextOptions;
    readonly TestData _data;
    
    public ObservationRepositoryTests()
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

    BisonDBContext CreateContext() => new BisonDBContext(_contextOptions);

    public void Dispose() => _connection.Dispose();
    
    
    // Tests for ReadObservation(id)

    [Fact]
    public async Task ReadObservation_ReturnsObservationForExistingID()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        long id = _data.ObservationWithComments.PostId;
        
        // Act
        var result = await repo.ReadObservation(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.PostId);
    }

    [Fact]
    public async Task ReadObservation_AuthorGetsLoadedCorrectly()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        long id = _data.ObservationWithComments.PostId;
        
        // Act
        var result = await repo.ReadObservation(id);
        
        // Assert
        Assert.NotNull(result.Author);
        Assert.Equal(_data.AuthorB.AuthorId, result.Author.AuthorId);
    }

    [Fact]
    public async Task ReadObservation_TaxonGetsLoadedCorrectly()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        long id = _data.ObservationWithComments.PostId;
        
        // Act
        var result = await repo.ReadObservation(id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(17)]
    [InlineData(34)]
    public async Task ReadObservation_VariousIds_ReturnMatchingObservation(int index)
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        var expected = _data.AuthorAObservations[index];
        
        // Act
        var result = await repo.ReadObservation(expected.PostId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.PostId, result.PostId);
    }
    
}