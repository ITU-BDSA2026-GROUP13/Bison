namespace Bison.Tests.IntegrationTests;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class ObservationRepositoryTests : IDisposable
{
    // Setup for the In-memory SQLite DB
    readonly SqliteConnection _connection;
    readonly DbContextOptions<BisonDBContext> _contextOptions;
    
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
        if (context.Database.EnsureCreated())
        {
            // TODO: create schema and seed db
        }

        context.SaveChanges();
    }

    BisonDBContext CreateContext() => new BisonDBContext(_contextOptions);

    public void Dispose() => _connection.Dispose();
    
    
    
    
}