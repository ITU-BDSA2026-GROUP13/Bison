namespace Bison;
using System.Text.Json.Serialization;

public class Proposal
{
    public long CheepID { get; set; }
    public string Author { get; set; }
    public string TaxonID { get; set; }
    public long Timestamp { get; set; }
    
    public Proposal() { }   // bruges af JSON
    public Proposal(long CheepID, string TaxonID)
    {
        this.Author = Environment.UserName;
        this.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        this.CheepID = CheepID;
        this.TaxonID = TaxonID;
    }
    
    [JsonConstructor]

    public Proposal(long CheepID, string Author, string TaxonID,  long Timestamp)
    {
        this.CheepID = CheepID;
        this.Author = Author;
        this.TaxonID = TaxonID;
        this.Timestamp = Timestamp;
    }
}