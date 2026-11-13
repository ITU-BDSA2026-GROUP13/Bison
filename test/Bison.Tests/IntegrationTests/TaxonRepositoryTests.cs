namespace Bison.Tests.IntegrationTests;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Repository;

public class TaxonRepositoryTests
{
    // Setup for the In-memory SQLite DB
    readonly SqliteConnection _connection;
    readonly DbContextOptions<BisonDBContext> _contextOptions;
    readonly TestData _data;
    
    public TaxonRepositoryTests()
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
    
    
    //Tests for GetTaxonById
    [Fact]
    public async Task GetTaxonById_ReturnedCorrectId()
    {
        string expected_dwc_TaxonID = "TEST:heron";
        using var context = CreateContext();
        var repo = new TaxonRepository(context);

        var result = await repo.GetTaxonById(expected_dwc_TaxonID);

        Assert.NotNull(result);
        Assert.Equal(expected_dwc_TaxonID, result.dwc_TaxonID);
    }

    [Fact]
    public async Task GetTaxonById_UsingNonExistingTaxon()
    {
        using var context = CreateContext();
        var repo = new TaxonRepository(context);

        var result = await repo.GetTaxonById("lars");

        Assert.Null(result);
    }
    
    
}