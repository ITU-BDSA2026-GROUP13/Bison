namespace Bison.FuzzTests;

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using ServerProgram = Service.ObservationService; // servers Program, not the client

public class BisonFuzzE2ETests : IClassFixture<WebApplicationFactory<ServerProgram>>
{
    private const string CheepPost = "/observation";
    private const string CheepsGet = "/observations";
    private const string CommentPost = "/comment";
    private const string CommentsGet = "/comments";

    private readonly HttpClient client;
    private readonly ITestOutputHelper output;

    public BisonFuzzE2ETests(WebApplicationFactory<ServerProgram> factory, ITestOutputHelper output)
    {
        var dir = Path.Combine(Path.GetTempPath(), $"bison_fuzz_{Guid.NewGuid()}");
        Directory.CreateDirectory(dir);
        Environment.SetEnvironmentVariable("BISON_OBSERVATION_DB", Path.Combine(dir, "observations.csv"));
        Environment.SetEnvironmentVariable("BISON_COMMENT_DB", Path.Combine(dir, "comments.csv"));
        Environment.SetEnvironmentVariable("BISON_PROPOSAL_DB", Path.Combine(dir, "proposals.csv"));

        client = factory.CreateClient();
        this.output = output;
    }

    [Fact]
    public async Task FuzzCheepsAndComments()
    {
        var seed = Environment.TickCount;
        output.WriteLine($"Fuzz seed: {seed}");
        var ran = new Random(seed);

        var oracle = new FuzzOracle(CheepsGet, CommentsGet);
        await oracle.LoadBaselineAsync(client);

        var cheepGen = new CheepGenerator(ran);
        var commentGen = new CommentGenerator(ran, () => oracle.PickCheepId(ran));

        for (int i = 0; i < 300; i++)
        {
            // Uden en kendt cheep kan vi ikke lave en gyldig kommentar.
            int kind = oracle.Cheeps.Count == 0 ? 0 : ran.Next(10);

            if (kind < 5) await PostCheepAsync(cheepGen, oracle, seed);
            else await PostCommentAsync(commentGen, oracle, seed);

            if (i % 50 == 49) await oracle.AssertMatchesServerAsync(client, seed);
        }

        await oracle.AssertMatchesServerAsync(client, seed);
    }

    private async Task PostCheepAsync(CheepGenerator gen, FuzzOracle oracle, int seed)
    {
        var cheep = gen.Generate();
        var res = await client.PostAsJsonAsync(CheepPost, cheep);

        Assert.True(res.IsSuccessStatusCode,
            $"[seed {seed}] POST {CheepPost} afvist: {res.StatusCode}, cheep: {cheep}");

        oracle.Cheeps.Add(cheep);
    }

    private async Task PostCommentAsync(CommentGenerator gen, FuzzOracle oracle, int seed)
    {
        var comment = gen.Generate();
        var res = await client.PostAsJsonAsync(CommentPost, comment);

        // Accepteret hvis og kun hvis cheepen findes.
        bool expectAccepted = oracle.KnowsCheep(comment.CheepID);

        Assert.True(res.IsSuccessStatusCode == expectAccepted,
            $"[seed {seed}] POST {CommentPost} (CheepID {comment.CheepID}): forventede accepteret={expectAccepted}, fik {res.StatusCode}");

        if (expectAccepted)
            oracle.Comments.Add(comment);
        else
            Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }
}