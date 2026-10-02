
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;
using Bison;
using System.Net;

public class EndpointTesting
{
    private readonly HttpClient client = new HttpClient();

    [Fact]
    public async Task TestGetObservationsRequest()
    {
        //Arrange
        string observationUrl = "http://localhost:5229/observations";
        
        //Act
        var response = await client.GetAsync(observationUrl);
        
        //Assert
        //statuscode ok?? (200).
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //Test if json
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType?.ToString());

        //If content of response of is empty, any object type would suffice. TA HJÆLPPPPP!
        var observations = await response.Content.ReadFromJsonAsync<List<Cheep>>();
        Assert.NotNull(observations);
        Assert.NotEmpty(observations);
    }
    
    [Fact]
    public async Task TestPostObservationRequest()
    {
        //Arrange
        Cheep cheep = new Cheep("i saw a goat", "farm");
        
        //Act
        var response = await client.PostAsJsonAsync("http://localhost:5229/observation", cheep);

        //Assert
        //statuscode ok?? (200).
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
    }
    
    [Fact]
    public async Task TestWrongObserationIDCommentsRequest()
    {
        //Arrange
        string commentUrl = "http://localhost:5229/comments?id=3934985748579548"; //No ID matches this
        
        //Act
        var response = await client.GetAsync(commentUrl);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); //Statuscode not found?
        //Assert.Equal(HttpStatusCode.OK, response.StatusCode); //Statuscode ok?
        // These two options depend on the intended behavior of the program
        
    }

    [Fact]
    public async Task TestPostCommentNonExistingObservationRequest()
    {
        //Arrange
        Comment comment = new Comment(6767676767676767, "Tester", "TestMessage", 1000);
        
        //Act
        var response = await client.PostAsJsonAsync("http://localhost:5229/comment", comment);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
