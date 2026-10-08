namespace Bison.Tests;

using System;
using System.Net;
using Xunit;
using Xunit.Abstractions;

public class BisonEndpointTests
{
    private readonly HttpClient client = new HttpClient();
    
    
    [Fact]
    public async Task TestGetAllObservationsRequest()
    {
        //Arrange
        string observationsUrl = "http://localhost:5273/obs";
        
        //Act
        var response = await client.GetAsync(observationsUrl);
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
    }
    
    [Fact]
    public async Task TestGetUserTimelineRequest()
    {
        //Arrange
        string userTimelineUrl = "http://localhost:5273/Lars";
        
        //Act
        var response = await client.GetAsync(userTimelineUrl);
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TestAccesingNonExistingPageRequest()
    {
        //Arrange
        string nonExistingPageUrl = "http://localhost:5273/Lars";
        
        //Act
        var response = await client.GetAsync(nonExistingPageUrl);
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); 
        // We may need to change existing behavior to fit BadRequest rather than NotFound.
        // Question is: Do we want to return a "Page Not Found" or an intended error message. 
    }

    [Fact]
    public async Task TestAllObservationsNotEmpty()
    {
        //Arrange
        string observationsUrl = "http://localhost:5273/obs";
        
        //Act
        var response = await client.GetAsync(observationsUrl);
        var body = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEmpty(body);
    }

    [Fact]
    public async Task TestGetSinglePageObservationRequest()
    {
        //Arrange
        string observationsUrl = "http://localhost:5273/ob/1";
        
        //Act
        var response = await client.GetAsync(observationsUrl);
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
    }
    
    //TODO: Test remaining getting endpoints (Comments, Proposals)
    
    //TODO: Test POSTING endpoints (All)
    
    
    
}