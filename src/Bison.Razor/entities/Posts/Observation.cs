namespace Bison;

public class Observation : Post
{
    public Taxon Taxon { get; set; }
    public string Location { get; set; }
    public List<Comment> Comments { get; set; } = new();
    public List<Proposal> Proposals { get; set; } = new();
    public string? ImageFile { get; set; } 
    public Taxon GetTaxon() => Taxon;
}