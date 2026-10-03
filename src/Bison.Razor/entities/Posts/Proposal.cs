namespace Bison;

using System;
using System.Text.Json.Serialization;

public record Proposal : UserAddition
{
    public string TaxonID { get; set; }
    
    public Proposal() { }   // bruges af JSON
    public Proposal(long ObservationID, string TaxonID)
    {
        this.Author = Environment.UserName;
        this.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        this.ObservationID = ObservationID;
        this.TaxonID = TaxonID;
    }
    
    [JsonConstructor]
    public Proposal(long ObservationID, string Author, string TaxonID,  long Timestamp)
    {
        this.ObservationID = ObservationID;
        this.Author = Author;
        this.TaxonID = TaxonID;
        this.Timestamp = Timestamp;
    }
}