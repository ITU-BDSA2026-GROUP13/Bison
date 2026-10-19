
namespace Bison.Tests;

using System;
using System.Net;
using Xunit;
using Xunit.Abstractions;
public class BisonEndpointTests
{
    /*
    private readonly HttpClient client = new HttpClient();
    
    
    // General
    [Fact]
    public async Task TestAccesingNonExistingPageRequest()
    {
        //Arrange
        string nonExistingPageUrl = "http://localhost:5273/67Lars67";
        
        //Act
        var response = await client.GetAsync(nonExistingPageUrl);
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); 
        // We may need to change existing behavior to fit BadRequest rather than NotFound.
        // Question is: Do we want to return a "Page Not Found" or an intended error message. 
    }
    
    
    // All Observations Pages
    [Fact]
    public async Task TestGetAllObservationsRequest()
    {
        //Arrange
        string observationsUrl = "http://localhost:5273/obs";
        
        //Act
        var response = await client.GetAsync(observationsUrl);
        var content =  await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Access OK?
        Assert.Contains("Public Timeline", content);
    }

    [Fact]
    public async Task TestGetMultiplePagesOnObservationsRequest()
    {
        // Arrange
        string observationsUrl = "http://localhost:5273/obs?taxonId=&page=2";
        
        // Act
        var response = await client.GetAsync(observationsUrl);
        var content = await response.Content.ReadAsStringAsync();
        
        // Act
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Access OK?
        Assert.Contains("Page 2", content);
    }

    [Fact]
    public async Task TestExceedingPageAmountOnObservationsRequest()
    {
        // Arrange
        string exceedingUrl = "http://localhost:5273/obs?taxonId=&page=999";
        
        // Act
        var response = await client.GetAsync(exceedingUrl);
        string content = await response.Content.ReadAsStringAsync();
        
        // Act
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Access OK?
        
        Assert.Contains("There are no observations so far", content);
    }
    
    [Fact]
    public async Task TestExtremeExceedingPageAmountOnObservationsRequest()
    {
        // THIS TEST FAILS! We somehow redirect back to start when we over-exceed. Is this the intended behavior?
        
        // Arrange
        string extremeExceedingUrl = "http://localhost:5273/obs?taxonId=&page=9999999999999999999999999";
        
        // Act
        var response = await client.GetAsync(extremeExceedingUrl);
        string content = await response.Content.ReadAsStringAsync();
        
        // Act
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Access OK?
        
        Assert.DoesNotContain("There are no observations so far", content); // Page redirects when exceeding the max. 
    }
    
    // All Observations Filtering
    [Fact]
    public async Task TestGetUserTimelineRequest()
    {
        //Arrange
        string userTimelineUrl = "http://localhost:5273/obs/1";
        string username = "Roger Histand";
        string otherUsername = "Nathan Sirmon";
        
        //Act
        var response = await client.GetAsync(userTimelineUrl);
        string content = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Access OK?
        
        Assert.Contains(username, content);
        Assert.DoesNotContain(otherUsername, content);
    }
    
    [Fact]
    public async Task TestTaxonFilteringOnObservationsRequest()
    {
        // Arrange
        string taxon = "Skestork";
        string taxonId = "MSTSNM:Arter:f3fa2bf9-f785-ea11-aa77-501ac539d1ea";
        string otherTaxon = "Dværghejre";
        string filteredUrl = "http://localhost:5273/obs?taxonId=" + taxonId;
        
        // Act
        var response = await client.GetAsync(filteredUrl);
        string content = await response.Content.ReadAsStringAsync();
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Access OK?
        
        Assert.Contains(taxon, content);
        Assert.DoesNotContain(otherTaxon, content);
    }
    
    [Fact]
    public async Task TestGetUserTimelineAndTaxonRequest()
    {
        //Arrange
        string username = "Roger Histand";
        string otherUsername = "Nathan Sirmon";
        string taxon = "Silkehejre";
        string taxonId = "MSTSNM:Arter:ca4d8cf8-f785-ea11-aa77-501ac539d1ea";
        string otherTaxon = "Dværghejre";
        string userTimelineUrl = "http://localhost:5273/obs/1?taxonId=" + taxonId;
        
        //Act
        var response = await client.GetAsync(userTimelineUrl);
        string content = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Access OK?
        
        Assert.Contains(username, content);
        Assert.DoesNotContain(otherUsername, content);
        
        Assert.Contains(taxon, content);
        Assert.DoesNotContain(otherTaxon, content);
    }
    

    
    // Single Observation Pages
    [Fact]
    public async Task TestGetSinglePageObservationRequest()
    {
        //Arrange
        string observationsUrl = "http://localhost:5273/ob/1";
        
        //Act
        var response = await client.GetAsync(observationsUrl);
        string content = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Access OK?
        
        Assert.Contains("at the lake", content);
    }

    [Fact]
    public async Task TestGetNonExistingSingleObRequest()
    {
        //Arrange
        string nonExistingSingleObUrl = "http://localhost:5273/ob/9761973649136471923466376617"; //Long non-existing id
        
        //Act
        var response = await client.GetAsync(nonExistingSingleObUrl);
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        // We may need to change existing behavior to fit BadRequest rather than NotFound.
        // Same question as wrong endpoint above
    }
    
    
    //TODO: Test POSTING endpoints (All) (If this is how they will be added)
 */   
}
