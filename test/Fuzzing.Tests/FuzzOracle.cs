namespace Bison.FuzzTests;

using System.Net.Http.Json;
using Xunit;

public class FuzzOracle
{
    private readonly string cheepsPath;
    private readonly string commentsPath;
    private readonly string proposalsPath;

    public FuzzOracle(string cheepsPath, string commentsPath, string proposalsPath)
    {
        this.cheepsPath = cheepsPath;
        this.commentsPath = commentsPath;
        this.proposalsPath = proposalsPath;
    }

    // What the server should contain, based on everything the fuzzer has sent
    public List<Cheep> Cheeps { get; } = new();
    public List<Comment> Comments { get; } = new();
    public List<Proposal> Proposals { get; } = new();

    public bool KnowsCheep(long id) => Cheeps.Any(c => c.CheepID == id);

    // ~90% known IDs, the rest random (likely nonexistent)
    public long PickCheepId(Random ran) =>
        Cheeps.Count > 0 && ran.NextDouble() < 0.9
            ? Cheeps[ran.Next(Cheeps.Count)].CheepID
            : ran.NextInt64(1_000_000_000, long.MaxValue);

    // The database may already contain data, so the oracle starts from the server's current state
    public async Task LoadBaselineAsync(HttpClient client)
    {
        var cheeps = await client.GetFromJsonAsync<List<Cheep>>(cheepsPath) ?? new();
        Cheeps.AddRange(cheeps);
        Comments.AddRange(await FetchCommentsAsync(client, cheeps));
        Proposals.AddRange(await FetchProposalsAsync(client, cheeps));
    }

    // Compares the oracle's expectation against what the server actually returns
    public async Task AssertMatchesServerAsync(HttpClient client, int seed)
    {
        var cheeps = await client.GetFromJsonAsync<List<Cheep>>(cheepsPath) ?? new();
        var comments = await FetchCommentsAsync(client, cheeps);
        var proposals = await FetchProposalsAsync(client, cheeps);

        // Records have value equality, so Assert.Equal/SequenceEqual compares content element-by-element
        Assert.True(Sorted(Cheeps).SequenceEqual(Sorted(cheeps)),
            $"[seed {seed}] Cheeps don't match ({Cheeps.Count} expected, {cheeps.Count} got)");
        Assert.True(Sorted(Comments).SequenceEqual(Sorted(comments)),
            $"[seed {seed}] Comments don't match ({Comments.Count} expected, {comments.Count} got)");
        Assert.True(Sorted(Proposals).SequenceEqual(Sorted(proposals)),
            $"[seed {seed}] Proposals don't match ({Proposals.Count} expected, {proposals.Count} got)");
    }

    private async Task<List<Comment>> FetchCommentsAsync(HttpClient client, IEnumerable<Cheep> cheeps)
    {
        var comments = new List<Comment>();
        // /comments requires ?id=, so we fetch comments per cheep
        foreach (var id in cheeps.Select(c => c.CheepID).Distinct())
            comments.AddRange(
                await client.GetFromJsonAsync<List<Comment>>($"{commentsPath}?id={id}") ?? new());
        return comments;
    }

    // /proposals also requires ?id=, same pattern as comments
    private async Task<List<Proposal>> FetchProposalsAsync(HttpClient client, IEnumerable<Cheep> cheeps)
    {
        var proposals = new List<Proposal>();
        foreach (var id in cheeps.Select(c => c.CheepID).Distinct())
            proposals.AddRange(
                await client.GetFromJsonAsync<List<Proposal>>($"{proposalsPath}?id={id}") ?? new());
        return proposals;
    }

    private static IEnumerable<Cheep> Sorted(IEnumerable<Cheep> xs) =>
        xs.OrderBy(c => c.CheepID);

    private static IEnumerable<Comment> Sorted(IEnumerable<Comment> xs) =>
        xs.OrderBy(c => c.CheepID).ThenBy(c => c.Timestamp).ThenBy(c => c.Message, StringComparer.Ordinal);

    private static IEnumerable<Proposal> Sorted(IEnumerable<Proposal> xs) =>
        xs.OrderBy(p => p.CheepID).ThenBy(p => p.Timestamp).ThenBy(p => p.TaxonID, StringComparer.Ordinal);
}