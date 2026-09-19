namespace Bison;
using System.Text.Json.Serialization;


public record Comment
{
    public long CheepID { get; set; }
    public string Author { get; set; }
    public string Message { get; set; }
    public long Timestamp { get; set; }

    public Comment() { }   // bruges af JSON
    public Comment(long CheepID, string Message)
    {
        this.Author = Environment.UserName;
        this.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        this.CheepID = CheepID;
        this.Message = Message;
    }
    
    [JsonConstructor]

    public Comment(long CheepID, string Author, string Message,  long Timestamp)
    {
        this.CheepID = CheepID;
        this.Author = Author;
        this.Message = Message;
        this.Timestamp = Timestamp;
    }
}