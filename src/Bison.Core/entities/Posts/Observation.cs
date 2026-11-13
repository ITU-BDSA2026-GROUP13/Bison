namespace Bison;

using System.Collections.Generic;

public class Observation : Post
{
    public Taxon Taxon { get; set; }
    public string Location { get; set; }
    public List<Comment> Comments { get; set; } = new();
    public List<Proposal> Proposals { get; set; } = new();

    public Taxon GetTaxon() => Taxon;
}