namespace Bison.FuzzTests;

public class CheepGenerator
{
    
    private const string Alphabet = "abcXYZ019 ,\";'æøåÆØÅ-_";

    private readonly Random ran;

    public CheepGenerator(Random ran)
    {
        this.ran = ran;
    }

    public Cheep Generate() => new Cheep(RandomText(ran), RandomText(ran));

    // Shared with CommentGenerator.
    public static string RandomText(Random ran)
    {
        var len = ran.Next(1, 40);
        return new string(Enumerable.Range(0, len)
            .Select(_ => Alphabet[ran.Next(Alphabet.Length)]).ToArray());
    }
}