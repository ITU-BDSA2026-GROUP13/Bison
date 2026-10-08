using Bison;
using Microsoft.Data.Sqlite;

namespace Bison.Tests;

public class DBFacadeTests
{
    private const int PageSize = 32;

    private readonly DBFacade db;

    private readonly string dbPath = Environment.GetEnvironmentVariable("BISONDBPATH") ??
        Path.Combine(Path.GetTempPath(), "bison.db");

    public DBFacadeTests()
    {
        db = new DBFacade(dbPath);
    }

    [Fact]
    public void GetObservationsReturnsMostRecentObservationsFirst()
    {
        // Arrange
        var observations = db.getObservations(1);
        var timestamps = new List<long>();
        
        // Act
        for (int i = 0; i < observations.Count(); i++)
        {
            timestamps.Add(long.Parse(observations[i].Timestamp));
        }

        //Assert
        Assert.Equal(PageSize, observations.Count);
        Assert.Equal(timestamps.OrderByDescending(timestamp => timestamp), timestamps);
    }

    [Fact]
    public void GetObservationsReturnsDifferentPages()
    {
        // Arrange + Act
        var firstPage = db.getObservations(1);
        var secondPage = db.getObservations(2);

        // Assert
        Assert.Equal(PageSize, firstPage.Count);
        Assert.Equal(PageSize, secondPage.Count);
        Assert.NotEqual(firstPage[0], secondPage[1]);
    }

    [Fact]
    public void GetObservationsByAuthorReturnsOnlyObservationsWithSameAuthor()
    {
        //Arrange
        var author = db.getObservations(1)[0].Author;
        
        //Act
        var observations = db.getObservations(author, 1);
        
        //Assert
        Assert.NotEmpty(observations);
        Assert.All(observations, observation => Assert.Equal(author, observation.Author));
    }
    
}
