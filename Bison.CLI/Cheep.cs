namespace Bison;

using System;

public record Cheep
{
    public long CheepID { get; set; }
    public string Author { get; set; }
    public string Observation { get; set; }
    public long Timestamp { get; set; }

    public Cheep(string Observation)
    {
        this.Observation = Observation;
        this.Author = Environment.UserName;
        this.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        this.CheepID = Math.Abs(this.Author.GetHashCode() + this.Observation.GetHashCode() + 
                       this.Timestamp.GetHashCode());
    }
}