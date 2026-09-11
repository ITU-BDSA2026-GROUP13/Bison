namespace Bison;

using System;

public record Cheep
{
    public long CheepID { get; set; }
    public string Author { get; set; }
    public string Observation { get; set; }
    public long Timestamp { get; set; }

    public Cheep(string Observation) // Used for writing to CSV
    {
        this.Observation = Observation;
        this.Author = Environment.UserName;
        this.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        this.CheepID = Math.Abs(this.Author.GetHashCode() + this.Observation.GetHashCode() + 
                       this.Timestamp.GetHashCode());
    }

    public Cheep(long CheepID, string Author, string Observation, long Timestamp) // Used for reading from CSV
    {
        this.CheepID = CheepID;
        this.Author = Author;
        this.Observation = Observation;
        this.Timestamp = Timestamp;
        
    }
}