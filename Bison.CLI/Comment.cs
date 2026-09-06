namespace Bison;

public record Comment
{
    long CheepID { get; set; }
    string Author { get; set; }
    string Comment { get; set; }
    long Timestamp { get; set; }

    public Comment(long CheepID, string Comment)
    {
        this.Author = Environment.UserName;
        this.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        this.CheepID = CheepID;
        this.Comment = Comment;
    }
}