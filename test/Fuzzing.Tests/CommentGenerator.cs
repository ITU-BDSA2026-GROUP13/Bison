namespace Bison.FuzzTests;

public class CommentGenerator
{
    private readonly Random ran;
    private readonly Func<long> pickCheepId;

    public CommentGenerator(Random ran, Func<long> pickCheepId)
    {
        this.ran = ran;
        this.pickCheepId = pickCheepId;
    }

    public Comment Generate() => new Comment(pickCheepId(), CheepGenerator.RandomText(ran));
}