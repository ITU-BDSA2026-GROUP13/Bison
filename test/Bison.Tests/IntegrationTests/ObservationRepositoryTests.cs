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


    // Tests for ReadObservation

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
        
        // Assert
        Assert.NotNull(result.Taxon);
        Assert.Equal(_data.ObservationWithComments.Taxon.dwc_TaxonID, result.Taxon.dwc_TaxonID);
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

    [Fact]
    public async Task ReadObservation_NonExsitingIdReturnsNull()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        long id = -1;
        
        // Act
        var result = await repo.ReadObservation(id);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ReadObservation_ReadingCommentReturnsNull()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        long id = _data.Comments[0].PostId;
        
        // Act
        var result = await repo.ReadObservation(id);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ReadObservation_ReadingProposalReturnsNull()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        long id = _data.Proposals[0].PostId;
        
        // Act
        var result = await repo.ReadObservation(id);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ReadObservation_TaxonWithoutVernacularNameStillLoads()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        long id = _data.UnnamedObservation.PostId;
        
        // Act
        var result = await repo.ReadObservation(id);
        
        // Assert
        Assert.NotNull(result.Taxon);
        Assert.Null(result.Taxon.VernacularName);
    }
    
    // Tests for GetObservationByTaxonAndPage
    [Theory]
    [InlineData(1, "TEST:swan", 2)]
    [InlineData(1, "TEST:heron", 32)]
    [InlineData(2, "TEST:heron", 3)]
    public async Task GetObservationByTaxonAndPage_TaxonReturnsCorrectAmountOfObservations(int page, string taxonId, int expected)
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        
        // Act
        var result = await repo.GetObservationByTaxonAndPage(taxonId, page);

        // Assert
        Assert.Equal(expected, result.Count);
    }

    [Fact]
    public async Task GetObservationByTaxonAndPage_NullTaxonReturnsAll_AndCheckSortedByTime()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        
        // Act
        var page1 = await repo.GetObservationByTaxonAndPage(null, 1);
        var page2 = await repo.GetObservationByTaxonAndPage(null, 2);
        
        // Assert
        Assert.Equal(32, page1.Count);
        Assert.Equal(6, page2.Count);
        for (int i = 1; i < page1.Count; i++)
        {
            Assert.True(page1[i-1].TimeStamp >= page1[i].TimeStamp);
        }
    }

    [Fact]
    public async Task GetObservationByTaxonAndPage_UnknownTaxonReturnsEmptyList()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        var unknownTaxonId = "ThisIsWeird";
        
        // Act
        var result = await repo.GetObservationByTaxonAndPage(unknownTaxonId, 1);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetObservationByTaxonAndPage_CorrectTaxonInWholeList()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        var expected = _data.Heron;
        var taxonId = "TEST:heron";

        // Act
        var result = await repo.GetObservationByTaxonAndPage(taxonId, 1);

        // Assert
        Assert.Equal(32, result.Count);
        Assert.All(result, o => Assert.Equal(expected.TaxonId, o.Taxon.TaxonId));
    }
    
    // Test GetObservationByTaxonAndAuthorAndPage

    [Fact]
    public async Task GetObservationByTaxonAndAuthorAndPage_AuthorAndTaxonReturnsMatchingObservations()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        var authorId = _data.AuthorB.AuthorId;
        var taxonId = "TEST:swan";
        
        // Act
        var result = await repo.GetObservationByTaxonAndAuthorAndPage(taxonId, authorId, 1);
        
        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetObservationByTaxonAndAuthorAndPage_NullTaxonReturnsAllFromAuthor()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        var authorId = _data.AuthorB.AuthorId;
        string taxonId = null;
        
        // Act
        var result = await repo.GetObservationByTaxonAndAuthorAndPage(taxonId, authorId, 1);
        
        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetObservationByTaxonAndAuthorAndPage_AuthorWithoutTaxonReturnsEmpty()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        var authorId = _data.AuthorA.AuthorId;
        var taxonId = _data.Swan.dwc_TaxonID;

        // Act
        var result = await repo.GetObservationByTaxonAndAuthorAndPage(taxonId, authorId, 1);

        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetObservationByTaxonAndAuthorAndPage_ResultsMatchBothAuthorAndTaxon()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        var authorId = _data.AuthorA.AuthorId;
        var taxonId = _data.Heron.dwc_TaxonID;

        // Act
        var result = await repo.GetObservationByTaxonAndAuthorAndPage(taxonId, authorId, 1);

        // Assert
        Assert.Equal(32, result.Count);
        Assert.All(result, o =>
        {
            Assert.Equal(authorId, o.Author.AuthorId);
            Assert.Equal(taxonId, o.Taxon.dwc_TaxonID);
        });
    }
    
    
    [Theory]
    [InlineData(1, 32)]
    [InlineData(2, 3)]
    public async Task GetObservationByTaxonAndAuthorAndPage_PaginationReturnsCorrectly(int page, int expectedCount)
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        var authorId = _data.AuthorA.AuthorId;
        var taxonId = _data.Heron.dwc_TaxonID;

        // Act
        var result = await repo.GetObservationByTaxonAndAuthorAndPage(taxonId, authorId, page);

        // Assert
        Assert.Equal(expectedCount, result.Count);
    }
    
    
    
    
    
    
    // Tests for methods we actually don't use... (Remembered this after writing them :((((( )
    
    // Tests for GetObservationByPage

    [Theory]
    [InlineData(1, 32)]
    [InlineData(2, 6)]
    [InlineData(3, 0)]
    public async Task GetObservationByPage_PageXReturnsListOfExpectedSize(int page, int expectedSize)
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        
        // Act
        var result = await repo.GetObservationsByPage(page);
        
        // Assert
        Assert.Equal(expectedSize, result.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task GetObservationByPage_PageBelowOneReturnsFirstPage(int page)
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        var expected = await repo.GetObservationsByPage(1);

        // Act
        var result = await repo.GetObservationsByPage(page);

        // Assert
        Assert.Equal(expected.Select(o => o.PostId), result.Select(o => o.PostId));
    }

    [Fact]
    public async Task GetObservationByPage_returnsSortedBasedOnNewestObservation()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        int page = 1;
        
        // Act
        var result = await repo.GetObservationsByPage(page);
        
        // Assert
        for (int i = 1; i < result.Count; i++)
        {
            Assert.True(result[i-1].TimeStamp >= result[i].TimeStamp);
        }
    }

    [Fact]
    public async Task GetObservationByPage_NoOverLapBetweenPages()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        
        // Act
        var page1 = await repo.GetObservationsByPage(1);
        var page2 = await repo.GetObservationsByPage(2);
        
        // Assert
        var page1Ids = page1.Select(o => o.PostId);
        var page2Ids = page2.Select(o => o.PostId);
        Assert.Empty(page1Ids.Intersect(page2Ids));
        Assert.Equal(38, page1Ids.Concat(page2Ids).Distinct().Count());
    }

    [Fact]
    public async Task GetObservationByPage_TaxonAndAuthorsAreIncluded()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        int page = 1;

        // Act
        var result = await repo.GetObservationsByPage(1);
        
        // Assert
        Assert.NotNull(result.Select(o => o.Taxon));
        Assert.NotNull(result.Select(o => o.Author));

    }
    
    
    // Tests for GetObservationByAuthorAndPage

    [Theory]
    [InlineData(1, 32)]
    [InlineData(2, 3)]
    public async Task GetObservationByAuthorAndPage_AuthorsPaginationReturnsCorrectly(int page, int expectedObservations)
    {
        // Arrange
        using var context = CreateContext();
        var repo = new ObservationRepository(context);
        var authorId = _data.AuthorA.AuthorId;
        
        // Act
        var result = await repo.GetObservationsByAuthorAndPage(authorId, page);
        
        // Assert
        Assert.Equal(expectedObservations, result.Count);
    }
}