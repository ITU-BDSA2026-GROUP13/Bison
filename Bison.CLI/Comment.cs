namespace Bison;

public record Comment
{
    public long CheepID { get; set; }
    public string Author { get; set; }
    public string Message { get; set; }
    public long Timestamp { get; set; }

    public Comment(long CheepID, string Message)
    {
        this.Author = Environment.UserName;
        this.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        this.CheepID = CheepID;
        this.Message = Message;
    }
}