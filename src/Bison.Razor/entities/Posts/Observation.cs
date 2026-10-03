namespace Bison;

using System;
using System.Text.Json.Serialization;

public record Observation : Post
{
    public long ObservationID { get; set; }
    public string Location { get; set; }
    public string ObservationMessage { get; set; }

    public Observation() { }   // bruges af JSON

    public Observation(string ObservationMessage, string Location) // Used for writing to CSV
    {
        this.ObservationMessage = ObservationMessage;
        this.Location = Location;
        this.Author = Environment.UserName;
        this.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        this.ObservationID = Math.Abs(this.Author.GetHashCode() + this.ObservationMessage.GetHashCode() +
                       this.Timestamp.GetHashCode());
    }

    [JsonConstructor]
    public Observation(long ObservationID, string Author, string ObservationMessage, string location, long Timestamp) // Used for reading from CSV
    {
        this.ObservationID = ObservationID;
        this.Author = Author;
        this.Location = location;
        this.ObservationMessage = ObservationMessage;
        this.Timestamp = Timestamp;
    }
}