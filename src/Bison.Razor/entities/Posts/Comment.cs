namespace Bison;

using System;
using System.Text.Json.Serialization;


public record Comment : UserAddition
{
    public string Message { get; set; }

    public Comment() { }   // bruges af JSON
    public Comment(long ObservationID, string Message)
    {
        this.Author = Environment.UserName;
        this.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        this.ObservationID = ObservationID;
        this.Message = Message;
    }
    
    [JsonConstructor]
    public Comment(long ObservationID, string Author, string Message,  long Timestamp)
    {
        this.ObservationID = ObservationID;
        this.Author = Author;
        this.Message = Message;
        this.Timestamp = Timestamp;
    }
}