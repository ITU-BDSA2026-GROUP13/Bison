namespace Bison.FuzzTests;

using System.Net.Http.Json;
using Bison;

public sealed class CheepGenerator
{
    private readonly Random random;

    public CheepGenerator(Random random)
    {
        this.random = random;
    }

    public Cheep Generate() =>
        new($"fuzz-{random.NextInt64()}", $"location-{random.Next(1000)}");
}

public sealed class CommentGenerator
{
    private readonly Random random;
    private readonly Func<long> pickCheepId;

    public CommentGenerator(Random random, Func<long> pickCheepId)
    {
        this.random = random;
        this.pickCheepId = pickCheepId;
    }

    public Comment Generate() =>
        new(pickCheepId(), $"fuzz-{random.NextInt64()}");
}

public sealed class FuzzOracle
{
    private readonly string cheepsPath;
    private readonly string commentsPath;

    public FuzzOracle(string cheepsPath, string commentsPath)
    {
        this.cheepsPath = cheepsPath;
        this.commentsPath = commentsPath;
    }

    public List<Cheep> Cheeps { get; } = new();
    public List<Comment> Comments { get; } = new();

    public bool KnowsCheep(long cheepId) => Cheeps.Any(cheep => cheep.CheepID == cheepId);

    public long PickCheepId(Random random) =>
        Cheeps.Count > 0 && random.NextDouble() < 0.9
            ? Cheeps[random.Next(Cheeps.Count)].CheepID
            : random.NextInt64(1, long.MaxValue);

    public async Task LoadBaselineAsync(HttpClient client)
    {
        Cheeps.AddRange(await client.GetFromJsonAsync<List<Cheep>>(cheepsPath) ?? new());
        await LoadCommentsAsync(client, Cheeps, Comments);
    }

    public async Task AssertMatchesServerAsync(HttpClient client, int seed)
    {
        var serverCheeps = await client.GetFromJsonAsync<List<Cheep>>(cheepsPath) ?? new();
        var serverComments = new List<Comment>();
        await LoadCommentsAsync(client, serverCheeps, serverComments);

        var expectedCheeps = Cheeps.OrderBy(cheep => cheep.CheepID);
        var actualCheeps = serverCheeps.OrderBy(cheep => cheep.CheepID);
        Assert.True(expectedCheeps.SequenceEqual(actualCheeps), $"[seed {seed}] cheep data differs from server");

        var expectedComments = OrderComments(Comments);
        var actualComments = OrderComments(serverComments);
        Assert.True(expectedComments.SequenceEqual(actualComments), $"[seed {seed}] comment data differs from server");
    }

    private async Task LoadCommentsAsync(HttpClient client, IEnumerable<Cheep> cheeps, List<Comment> destination)
    {
        foreach (var cheep in cheeps.DistinctBy(cheep => cheep.CheepID))
        {
            var path = $"{commentsPath}?id={cheep.CheepID}";
            destination.AddRange(await client.GetFromJsonAsync<List<Comment>>(path) ?? new());
        }
    }

    private static IOrderedEnumerable<Comment> OrderComments(IEnumerable<Comment> comments) =>
        comments.OrderBy(comment => comment.CheepID)
            .ThenBy(comment => comment.Timestamp)
            .ThenBy(comment => comment.Message);
}