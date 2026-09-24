namespace Bison;
using CsvHelper.Configuration;

public class Taxon
{
    public string TaxonID { get; set; } = ""; // = "" makes sure the warning for non nullable doesnt appear
    public string ParentNameUsageID { get; set; } = "";
    public string TaxonRank { get; set; } = "";
    public string ScientificName { get; set; } = "";
    public string? VernacularName { get; set; } //The name in danish
    
    // Does not get read from csv
    public Taxon? SuperTaxon { get; set; }
    public List<Taxon> SubTaxons { get; set; } = new();



}

/*
 * Since I don't want to call the fields in Taxon the full header names,
 * this is needed to tell CsvHelper, which headers map to which fields.
 */
public sealed class TaxonMap : ClassMap<Taxon>
{
    public TaxonMap()
    {
        Map(m => m.TaxonID).Name("dwc:taxonID");
        Map(m => m.ParentNameUsageID).Name("dwc:parentNameUsageID");
        Map(m => m.TaxonRank).Name("dwc:taxonRank");
        Map(m => m.ScientificName).Name("dwc:scientificName");
        Map(m => m.VernacularName).Name("dwc:vernacularName");
    }
}