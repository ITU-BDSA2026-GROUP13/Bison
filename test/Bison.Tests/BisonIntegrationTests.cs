namespace Bison.Tests;

using System.Net;
using Xunit;

public class BisonIntegrationTests
{
    private readonly HttpClient client = new HttpClient();
    
    [Fact]
    public async Task TestObsPageContainsContent()
    {
        //Arrange
        string observationsUrl = "http://localhost:5273/obs";
        
        //Act
        var response = await client.GetAsync(observationsUrl);
        var html = await response.Content.ReadAsStringAsync();
        
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
        var html = await rightResponse.Content.ReadAsStringAsync();
        
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
        var body = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.NotEmpty(body);
    }
    
    [Fact]
    public async Task TestObRequestContainsCommentsAndProposalSegments()
    {
        //Arrange
        var observationId = 1;
        string observationsUrl = $"http://localhost:5273/ob/{observationId}";
        
        //Act
        var response = await client.GetAsync(observationsUrl);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Contains("<h3> Proposals </h3>", html);
        Assert.Contains("<h3> Comments </h3>", html);
    }
    
    [Fact]
    public async Task TestObsForSingleObservationsNotEqual()
    {
        //Arrange
        var observationId1 = 1;
        var observationId2 = 2;

        string observationsUrl = $"http://localhost:5273/ob/{observationId1}";
        string observationsUrl2 = $"http://localhost:5273/ob/{observationId2}";
        
        //Act
        var response1 = await client.GetAsync(observationsUrl);
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        var response2 = await client.GetAsync(observationsUrl2);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

        var html1 = await response1.Content.ReadAsStringAsync();
        var html2 = await response2.Content.ReadAsStringAsync();
        
        //Assert
        Assert.NotEqual(html1, html2);
    }
    
    // TODO: Test Comments and Proposals can be posted, and exist after posting.
    
    
    //----Dont know if these are UnitTests----//
    [Fact]
    public async Task TestGetObs()
    {
        //Arrange
        var observationId1 = 1;

        var observationViewModel1 = new ObservationViewModel(observationId1, "Lars", "Jeg så en ko", "Isen", 1000);
        
        //Assert
        Assert.Equal(observationViewModel1.GetObservationViewModels(observationId1), (observationId1, "Lars", "Jeg så en ko", "Isen", 1000));
    }
    // a test to see if the razor page is the same for the observation 1 and 00001
    [Fact]
    public async Task TestObsGetFor1and00001()
    {
        //Arrange
        var observationId1 = 1;
        var observationId2 = 00001;

        var observationViewModel1 = new ObservationViewModel(observationId1, "Lars", "Jeg så en ko", "Isen", 1000);
        var observationViewModel2 = new ObservationViewModel(observationId2, "Karina", "Det er en hest", "Landro", 2000);
        
        //Assert
        Assert.Equal(observationViewModel1.GetObservationFromId(observationId1), (observationId2, "Karina", "Det er en hest", "Landro", 2000));
    }

    [Fact]
    public async Task TestGetComments()
    {
        //Arrange
        var observationId = 1;
        var commentId1 = 1;
        var commentId2 = 2;

        var observationViewModel = new ObservationViewModel(observationId, "Lars", "Jeg så en ko", "Isen", 1000);
        var commentViewModel1 = new CommentViewModel(commentId1, observationId ,"Karina","Det passer bare slet ikke",2000);
        var commentViewModel2 = new CommentViewModel(commentId2, observationId ,"Bo", "Det er jo en Hest", 2500);
    
        //Assert
        Assert.Contains(observationViewModel.GetCommentViewModels(observationId),(commentId1, observationId ,"Karina","Det passer bare slet ikke",2000));
        Assert.Contains(observationViewModel.GetCommentViewModels(observationId), (commentId2, observationId ,"Bo", "Det er jo en Hest", 2500));
    }

    public async Task TestGetProposals()
    {
        //Arrange
        var observationId = 1;
        var proposalId1 = 1;
        var proposalId2 = 2;

        var observationViewModel = new ObservationViewModel(observationId, "Lars", "Jeg så en ko", "Isen", 1000);
        var proposalViewModel1 = new ProposalViewModel(proposalId1, observationId ,"Lars","MSTSNM9913","Bos taurus ");
        var proposalViewModel2 = new ProposalViewModel(proposalId2, observationId ,"Bo", "MSTSNM9796", "Equus caballus");
    
        //Assert
        Assert.Contains(observationViewModel.GetCommentViewModels(observationId),(proposalId1, observationId ,"Lars","MSTSNM9913","Bos taurus "));
        Assert.Contains(observationViewModel.GetCommentViewModels(observationId), (proposalId2, observationId ,"Bo", "MSTSNM9796", "Equus caballus"));
    }
    
}