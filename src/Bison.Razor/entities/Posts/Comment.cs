namespace Bison;

using System;
using System.Text.Json.Serialization;


public record Comment : UserAddition
{
    public string Message { get; set; }

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