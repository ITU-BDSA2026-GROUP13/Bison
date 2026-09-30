namespace Bison.FuzzTests;

using System.Net;
using System.Net.Http.Json;
using Bison;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using ServerProgram = Service.ObservationService; // server's Program marker, not the client's

public class BisonFuzzE2ETests
{
    private const string CheepPost = "/observation";
    private const string CheepsGet = "/observations";
    private const string CommentPost = "/comment";
    private const string CommentsGet = "/comments";
    private const string ProposalPost = "/proposal";
    private const string ProposalsGet = "/proposals";

    private readonly string dir;
    private readonly WebApplicationFactory<ServerProgram> factory;
    private readonly HttpClient client;
    private readonly ITestOutputHelper output;
    private readonly Taxonomy taxonomy;

    public BisonFuzzE2ETests( ITestOutputHelper output)
    {
        this.output = output;
        dir = Path.Combine(Path.GetTempPath(), $"bison_fuzz_{Guid.NewGuid()}");
        Directory.CreateDirectory(dir);

        // To make sure 
        //if (File.Exists(Path.Combine(dir, "observations.csv"))) File.Delete(Path.Combine(dir, "observations.csv"));
        //if (File.Exists(Path.Combine(dir, "comment.csv"))) File.Delete(Path.Combine(dir, "comment.csv"));
        //if (File.Exists(Path.Combine(dir, "proposals.csv"))) File.Delete(Path.Combine(dir, "proposals.csv"));
        
        // Point the server at temp CSV files so the fuzzer doesn't touch the real database
        Environment.SetEnvironmentVariable("BISON_OBSERVATION_DB", Path.Combine(dir, "observations.csv"));
        Environment.SetEnvironmentVariable("BISON_COMMENT_DB", Path.Combine(dir, "comments.csv"));
        Environment.SetEnvironmentVariable("BISON_PROPOSAL_DB", Path.Combine(dir, "proposals.csv"));
        
        factory = new WebApplicationFactory<ServerProgram>();
        client = factory.CreateClient();
        this.output = output;

        taxonomy = new Taxonomy();
        taxonomy.Taxonloader(); // regular method, not a constructor, so it must be called explicitly
    }

    // Each seed below runs as its own named test in the Test Explorer, and can be
    // rerun individually to reproduce a specific failure.
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(42)]
    [InlineData(1234567)]
    public async Task FuzzCheepsCommentsAndProposals(int seed)
    {
        await RunFuzzRun(seed);
    }

    // Extra, fully random test: seed changes every run, so it explores new
    // combinations each time instead of always repeating the fixed seeds above.
    [Fact]
    public async Task FuzzCheepsCommentsAndProposals_RandomSeed()
    {
        var seed = Environment.TickCount;
        await RunFuzzRun(seed);
    }

    private async Task RunFuzzRun(int seed)
    {
        output.WriteLine($"Fuzz seed: {seed}");
        var ran = new Random(seed);

        var oracle = new FuzzOracle(CheepsGet, CommentsGet, ProposalsGet);
        await oracle.LoadBaselineAsync(client);
        output.WriteLine($"Baseline: {oracle.Cheeps.Count} cheeps");
        
        var cheepGen = new CheepGenerator(ran);
        var commentGen = new CommentGenerator(ran, () => oracle.PickCheepId(ran));
        var proposalGen = new ProposalGenerator(ran, () => oracle.PickCheepId(ran), taxonomy);

        for (int i = 0; i < 300; i++)
        {
            int kind = ran.Next(10);

            if (kind < 4) await PostCheepAsync(cheepGen, oracle, seed);
            else if (kind < 7) await PostCommentAsync(commentGen, oracle, seed);
            else if (kind < 9) await PostRandomProposalAsync(proposalGen, oracle, seed);
            else if (oracle.Proposals.Count > 0) await PostMutatedProposalAsync(proposalGen, oracle,ran, seed);
            else await PostRandomProposalAsync(proposalGen, oracle, seed);

            if (i % 50 == 49) await oracle.AssertMatchesServerAsync(client, seed);
        }

        await oracle.AssertMatchesServerAsync(client, seed);
    }

    private async Task PostCheepAsync(CheepGenerator gen, FuzzOracle oracle, int seed)
    {
        var cheep = gen.Generate();
        var res = await client.PostAsJsonAsync(CheepPost, cheep);

        Assert.True(res.IsSuccessStatusCode,
            $"[seed {seed}] POST {CheepPost} rejected: {res.StatusCode}, cheep: {cheep}");

        oracle.Cheeps.Add(cheep);
    }

    private async Task PostCommentAsync(CommentGenerator gen, FuzzOracle oracle, int seed)
    {
        var comment = gen.Generate();
        var res = await client.PostAsJsonAsync(CommentPost, comment);

        // Only valid if the referenced cheep exists
        bool expectAccepted = oracle.KnowsCheep(comment.CheepID);

        Assert.True(res.IsSuccessStatusCode == expectAccepted,
            $"[seed {seed}] POST {CommentPost} (CheepID {comment.CheepID}): expected accepted={expectAccepted}, got {res.StatusCode}");

        if (expectAccepted)
            oracle.Comments.Add(comment);
        else
            Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    private async Task PostRandomProposalAsync(ProposalGenerator gen, FuzzOracle oracle, int seed)
    {
        var proposal = gen.GenerateRandom();
        var res = await client.PostAsJsonAsync(ProposalPost, proposal);

        // Server only validates CheepID (no Taxonomy service in DI), not TaxonID
        bool expectAccepted = oracle.KnowsCheep(proposal.CheepID);

        Assert.True(res.IsSuccessStatusCode == expectAccepted,
            $"[seed {seed}] POST {ProposalPost} (CheepID {proposal.CheepID}): expected accepted={expectAccepted}, got {res.StatusCode}");

        if (expectAccepted)
            oracle.Proposals.Add(proposal);
        else
            Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    private async Task PostMutatedProposalAsync(ProposalGenerator gen, FuzzOracle oracle,Random ran,int seed)
    {
        // Pick an existing proposal and re-send it with a different taxon ID
        var original = oracle.Proposals[ran.Next(oracle.Proposals.Count)];
        var proposal = gen.Mutate(original);
        var res = await client.PostAsJsonAsync(ProposalPost, proposal);

        bool expectAccepted = oracle.KnowsCheep(proposal.CheepID);

        Assert.True(res.IsSuccessStatusCode == expectAccepted,
            $"[seed {seed}] POST mutated {ProposalPost} (CheepID {proposal.CheepID}): expected accepted={expectAccepted}, got {res.StatusCode}");

        if (expectAccepted)
            oracle.Proposals.Add(proposal);
        else
            Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }
}