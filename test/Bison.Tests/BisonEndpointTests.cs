namespace Bison.Tests;

using System;
using System.Net;
using Xunit;
using Xunit.Abstractions;

public class BisonEndpointTests
{
    private readonly HttpClient client = new HttpClient();
    
    
    [Fact]
    public async void TestGetAllObservationsRequest()
    {
        //Arrange
        string observationsUrl = "http://localhost:5273/obs";
        
        //Act
        var response = await client.GetAsync(observationsUrl);
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
    }
    
    [Fact]
    public async void TestGetUserTimelineRequest()
    {
        //Arrange
        string userTimelineUrl = "http://localhost:5273/Lars";
        
        //Act
        var response = await client.GetAsync(userTimelineUrl);
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    
    
    
}