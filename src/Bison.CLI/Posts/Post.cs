namespace Bison;

public abstract record Post
{
    public string Author { get; set; }
    public long Timestamp { get; set; }
}