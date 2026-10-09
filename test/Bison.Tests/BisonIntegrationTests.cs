namespace Bison.Tests;

using System.Net;
using Bison;
using Xunit;

public class BisonIntegrationTests : IDisposable
{
    private readonly HttpClient client = new HttpClient();

    private readonly DBFacade db;

    private readonly string dbPath = Path.Combine(Path.GetTempPath(), $"bison-{Guid.NewGuid():N}.db");

    public BisonIntegrationTests()
    {
        db = new DBFacade(dbPath);
    }

    public void Dispose()
    {
        File.Delete(dbPath);
    }
    
    [Fact]
    public async Task TestObsPageContainsContent()
    {
        //Arrange
        string observationsUrl = "http://localhost:5273/obs";
        
        //Act
        var response = await client.GetAsync(observationsUrl);
        string html = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Contains("<title>Bison</title>", html);
        Assert.Contains("<h2> Public Timeline </h2>", html);
    }

    [Fact]
    public async Task TestClientDoesNotCrashOnWrongUrls()
    {
        //Arrange
        string existingUrl = "http://localhost:5273/obs";
        string nonExistingUrl = "http://localhost:5273/observations";
        
        //Act
        var wrongResponse = await client.GetAsync(nonExistingUrl);
        var rightResponse = await client.GetAsync(existingUrl);
        string html = await rightResponse.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, wrongResponse.StatusCode); // Couldn't find wrong url
        Assert.NotEqual(HttpStatusCode.InternalServerError, wrongResponse.StatusCode); // Client not crashed?
        
        Assert.Equal(HttpStatusCode.OK, rightResponse.StatusCode); // Right url still operates
        Assert.Contains("<title>Bison</title>", html); // Right url still contains content
    }
    
    [Fact]
    public async Task TestAllObservationsNotEmpty()
    {
        //Arrange
        string observationsUrl = "http://localhost:5273/obs";
        
        //Act
        var response = await client.GetAsync(observationsUrl);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        string body = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.NotEmpty(body);
    }
    
    [Fact]
    public async Task TestObRequestContainsCommentsAndProposalSegments()
    {
        //Arrange
        int observationId = 1;
        string observationsUrl = $"http://localhost:5273/ob/{observationId}";
        
        //Act
        var response = await client.GetAsync(observationsUrl);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        string html = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Contains("<h3>Proposals</h3>", html);
        Assert.Contains("<h3>Comments</h3>", html);
    }
    
    [Fact]
    public async Task TestObsForSingleObservationsNotEqual()
    {
        //Arrange
        int observationId1 = 1;
        int observationId2 = 2;

        string observationsUrl = $"http://localhost:5273/ob/{observationId1}";
        string observationsUrl2 = $"http://localhost:5273/ob/{observationId2}";
        
        //Act
        var response1 = await client.GetAsync(observationsUrl);
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        var response2 = await client.GetAsync(observationsUrl2);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

        string html1 = await response1.Content.ReadAsStringAsync();
        string html2 = await response2.Content.ReadAsStringAsync();
        
        //Assert
        Assert.NotEqual(html1, html2);
    }
    
    // TODO: Test Comments and Proposals can be posted, and exist after posting.
    
    
    //----Dont know if these are UnitTests----//
     
    [Fact]
    public async Task TestGetObs()
    {
        //Arrange
        long observationId = 67;
        string userName = "Mette";
        string observationMessage = "Squacco Heron on the pond at the edge of town. Hunts alone along the edge of the water.";
        string timeStamp = "1774916365";

        //Act
        var observation = db.getObservationFromId(observationId);

        //Assert
        Assert.Equal(userName, observation.Author);
        Assert.Equal(observationMessage, observation.Message);
        Assert.Equal(timeStamp, observation.Timestamp);
    }

    // a test to see if the razor page is the same for the observation 1 and 00001
    [Fact]
    public async Task TestObsGetFor1and00001()
    {
        //Arrange
        long observationId1 = 67;
        long observationId2 = 00000067;

        //Act
        var observation1 = db.getObservationFromId(observationId1);
        var observation2 = db.getObservationFromId(observationId2);

        //Assert
        Assert.Equal(observation1.Author, observation2.Author);
        Assert.Equal(observation1.Message, observation1.Message);
        Assert.Equal(observation1.Timestamp, observation1.Timestamp);
        Assert.Equal(observation1, observation2);
    }

    [Fact]
    public async Task TestGetComments()
    {
        //Arrange
        var observationId = 12;

        var commentId1 = 1;
        var commentId2 = 2;
    
        //Act
        var observationId12_comments = db.GetCommentViewModels(observationId);
        
        //Assert
        bool foundId1 = false;
        bool foundId2 = false;
        foreach (CommentViewModel comment in observationId12_comments )
        {
            if (comment.Comment_id == commentId1) foundId1 = true;
            if (comment.Comment_id == commentId2) foundId2 = true;
        }

        Assert.True(foundId1);
        Assert.True(foundId2);

        foreach (CommentViewModel comment in observationId12_comments)
        {
            Assert.Equal(observationId, comment.Observation_id);
        }
        
    }

    public async Task TestGetProposals()
    {
        //Arrange
        var observationId = 12;
        var proposalId = 1;
    
        //Act
        var observationId12_proposals = db.GetProposalViewModels(observationId);
        
        //Assert
        Assert.Equal(observationId, observationId12_proposals[0].Observation_id);
        Assert.Equal(proposalId, observationId12_proposals[0].Proposal_id);
    }
    
}