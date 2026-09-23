namespace Bison;

using CsvHelper;
using System.Globalization;

public class Taxonomy
{
    public List<Taxon> Taxons { get; set; } = new List<Taxon>();

    
    
    string csvFilePath = "../../Resources/joined.csv";
    public void Taxonloader()
    {
        using var reader = new StreamReader(csvFilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<TaxonMap>();
        Taxons = csv.GetRecords<Taxon>().ToList();
    }

    public void PrintTaxonomy()
    {
        foreach (var taxon in Taxons)
        {
            Console.WriteLine($"ID: {taxon.TaxonID}, Name: {taxon.VernacularName}, ParentID: {taxon.ParentNameUsageID}");
        
        }
    
    }
    public Taxon getTaxonByID(string id)
    {
        return Taxon;
    }

    public Taxon getTaxonByDanishName(string danishName)
    {
        return Taxon;
    }

    public Taxon getTaxonParentID(string parentID)
    {
        return Taxon;
    }

    public List<Taxon> getTaxonChildren(Taxon parent)
    {
        return List<Taxon>;
    }
}