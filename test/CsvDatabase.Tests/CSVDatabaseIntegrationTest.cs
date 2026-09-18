namespace CsvDatabase.Tests;

using System;
using Xunit;
using Bison;
using SimpleDB;

public class CSVDatabaseIntegrationTest
{
    readonly string testFileNameCheep = "csvDatabaseIntegrationTest_db.csv";
    readonly string testFileNameComment = "csvDatabaseIntegrationTest_Comment_db.csv";
    readonly CSVDatabase<Cheep> cheeps;
    readonly CSVDatabase<Comment> comments;
    Cheep exampleCheep;
    Comment exampleComment;
    
    public CSVDatabaseIntegrationTest()
    {
        if (File.Exists(testFileNameCheep)) File.Delete(testFileNameCheep);
        if (File.Exists(testFileNameComment)) File.Delete(testFileNameComment);
        cheeps = new CSVDatabase<Cheep>(testFileNameCheep);
        comments = new CSVDatabase<Comment>(testFileNameComment);

        exampleCheep = new Cheep("The answer to everything and Nothing", "Rued Langaardsvej 7");
        exampleComment = new Comment(exampleCheep.CheepID, "OMG IS THIS TRUE??!!");
    }
    
    [Fact]
    public void canReceiveObservationAfterItsStored()
    {
        // Adds new record to DB
        cheeps.Store(exampleCheep);
        
        // Reads observations in DB, and stores first observation in variable
        var listRead =  cheeps.Read().ToList();
        var observation = listRead[0];
        
        // Asserts
        Assert.Equal(observation, exampleCheep);
    }

    [Fact]
    public void canReceiveCommentAfterItsStored()
    {
        comments.Store(exampleComment);
        
        var listRead = comments.Read().ToList();
        var comment = listRead[0];
        
        // Asserts
        Assert.Equal(comment, exampleComment);
    }

    [Fact]
    public void DataStoredInOneInstance_IsReadableInNewInstance_PointingAtTheSameFile()
    {
        // Store data via the first instance
        cheeps.Store(exampleCheep);
        comments.Store(exampleComment);
        
        // Create second instance pointing at same file
        var secondCheepDB = new CSVDatabase<Cheep>(testFileNameCheep);
        var secondCommentDB = new CSVDatabase<Comment>(testFileNameComment);
        
        
        // Read from first instance
        var firstCheepResult = cheeps.Read().ToList()[0];
        var firstCommentResult = comments.Read().ToList()[0];
        
        // Read from second instance
        var cheepResult = secondCheepDB.Read().ToList()[0];
        var commentResult =  secondCommentDB.Read().ToList()[0];
        
        // Asserts
        Assert.Equal(firstCheepResult, cheepResult);
        Assert.Equal(firstCommentResult, commentResult);
    }
    
    [Fact]
    public void EmptyDBReturnsEmptyEnumerable()
    {
        var DB = new CSVDatabase<Cheep>("EmptyDB.csv");
        var dbRead = DB.Read();
        Assert.Empty(dbRead);
    }
    
    [Fact]
    public void ReadLimitReturnsEnumerableWithLimit()
    {
        string readLimitFileName = "ReadLimitDB.csv";
        if (File.Exists(readLimitFileName)) File.Delete(readLimitFileName);
        var DB = new CSVDatabase<Cheep>(readLimitFileName);
        DB.Store(new Cheep("First Observation", "Rued Langaardsvej 7"));
        DB.Store(new Cheep("Second Observation", "Rued Langaardsvej 7"));
        DB.Store(new Cheep("Third Observation", "Rued Langaardsvej 7"));
        DB.Store(new Cheep("Fourth Observation", "Rued Langaardsvej 7"));
        
        var dbRead = DB.Read(1);
        Assert.Single(dbRead);
        
        dbRead = DB.Read(2);
        Assert.Equal(2, dbRead.Count());
        
        dbRead = DB.Read(3);
        Assert.Equal(3, dbRead.Count());
        
        dbRead = DB.Read();
        Assert.Equal(4, dbRead.Count());
    }
}