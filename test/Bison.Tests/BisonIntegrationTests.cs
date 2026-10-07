namespace Bison.Tests;

using System.Net;
using Xunit;

public class BisonIntegrationTests
{
    private readonly HttpClient client = new HttpClient();
    
    [Fact]
    public async void TestObsPageContainsContent()
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
    public async void TestClientDoesNotCrashOnWrongUrls()
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
    
    
    // TODO: Test Comments and Proposals can be posted, and exist after posting. 
    
    
}