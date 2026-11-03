namespace Bison.Tests.IntegrationTests;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Repository;
using Xunit.Abstractions;
using Xunit.Sdk;

public class ProposalRepositoryTests
{
    // Setup for the In-memory SQLite DB
    readonly SqliteConnection _connection;
    readonly DbContextOptions<BisonDBContext> _contextOptions;
    readonly TestData _data;

    public ProposalRepositoryTests()
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
        

        context.SaveChanges();
    }


    [Fact]
    public async Task CreateProposal()
    {
        //Arrange 
        using var context = CreateContext();
        var repo = new ProposalRepository(context);
        
        //To make sure that EF core does not try to insert duplicate key
        var observation = await context.Observations
            .SingleAsync(o => o.PostId == _data.UnnamedObservation.PostId);
        var heron = await context.Taxons
            .SingleAsync(t => t.dwc_TaxonID == _data.Heron.dwc_TaxonID);
        var author = await context.Authors
            .SingleAsync(a => a.UserName == "AuthorC");
        
        var newProposal = new Proposal
        {
            Author = author, 
            Text = "This here is a proposal", 
            TimeStamp = new DateTime(2026, 1, 1, 12, 0, 0), 
            Taxon = heron,
            Observation = observation
        };

        //Act
        Assert.True(repo.GetProposalsByObsId(observation.PostId).Result.Count == 0);
        
        await repo.CreateProposal(newProposal);
        await context.SaveChangesAsync();
        
        //Needs to update our context.
        using var assertContext = CreateContext();
        var assertRepo = new ProposalRepository(assertContext);
        var proposals = await assertRepo.GetProposalsByObsId(observation.PostId);

        var proposal = Assert.Single(proposals);
        Assert.Equal("This here is a proposal", proposal.Text);
    }
    
    [Fact]
    public async Task GetProposalsByObsId()
    {
        //Arrange 
        using var context = CreateContext();
        var repo = new ProposalRepository(context);
        
        
        //Act
        var observationWith1Proposal = repo.GetProposalsByObsId(_data.ObservationWithComments.PostId).Result;
        var observationWith0Proposal = repo.GetProposalsByObsId(_data.UnnamedObservation.PostId).Result;
        
        //Assert
        Assert.True(observationWith1Proposal.Count == 1);
        Assert.True(observationWith1Proposal[0].Text == "Maybe this one?");
        Assert.False(observationWith1Proposal[0].Text == "Looks like a heron");

        Assert.True(observationWith0Proposal.Count == 0);
    }
    
    
    
    BisonDBContext CreateContext() => new BisonDBContext(_contextOptions);
    public void Dispose() => _connection.Dispose();
}