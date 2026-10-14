namespace Bison;

public class Taxon
{
    public long TaxonId { get; set; }
    public string dwc_TaxonId { get; set; } // The very long Id
    public string? VernacularName { get; set; } //The name in danish
    public Taxon? Parent { get; set; }
    public List<Taxon> Children { get; set; } = new();
}