
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
        var response = await client.GetAsync("http://localhost:5229/observations");
        
        //statuscode ok?? (200).
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //Test if json
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType?.ToString());


        //If content of response of is empty, any object type would suffice. TA HJÆLPPPPP!
        var observations = await response.Content.ReadFromJsonAsync<List<Cheep>>();
        Assert.NotNull(observations);
    }

    [Fact]
    public async Task TestPostObservationRequest()
    {
        Cheep cheep = new Cheep("i saw a goat", "farm");
        var response = await client.PostAsJsonAsync("http://localhost:5229/observation", cheep);

        //statuscode ok?? (200).
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
}
