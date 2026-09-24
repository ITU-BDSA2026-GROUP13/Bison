namespace Bison;

using CsvHelper;
using System.Globalization;
using System.IO;
using System.Reflection;

public class Taxonomy
{
    private List<Taxon> Taxons { get; set; } = new List<Taxon>();
    private readonly Dictionary<string, Taxon> lookupById = new();
    private readonly Dictionary<string, Taxon> lookupByVernacularName = new();

    
    string csvEmbeddedResourcePath = "Bison.CLI.joined.csv";
    
    public void Taxonloader()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream(csvEmbeddedResourcePath);
        var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<TaxonMap>();
        Taxons = csv.GetRecords<Taxon>().ToList();
        
        // Fill the Dictionaries
        foreach (var taxon in Taxons)
        {
            lookupById.Add(taxon.TaxonID, taxon);
            if (!string.IsNullOrWhiteSpace(taxon.VernacularName))
            {
                lookupByVernacularName.Add(taxon.VernacularName, taxon);
            }
        }
        
        // Populate SubTaxon list and assing SuperTaxons
        foreach (var taxon in Taxons)
        {
            if (lookupById.TryGetValue(taxon.ParentNameUsageID, out Taxon? parent))
            {
                taxon.SuperTaxon = parent;
                parent.SubTaxons.Add(taxon);
            }
        }
    }

    public void PrintTaxonomy()
    {
        foreach (var taxon in Taxons)
        {
            string parentID;
            if (taxon.SuperTaxon != null) parentID = taxon.SuperTaxon.TaxonID;
            else parentID = "None";
            
            Console.WriteLine($"ID: {taxon.TaxonID}, Name: {taxon.VernacularName}, ParentID: {taxon.ParentNameUsageID}, SuperTaxon: {parentID}");
        
        }
    
    }
    public Taxon? GetTaxonById(string id)
    {
        
        return lookupById.TryGetValue(id, out Taxon? taxon) ? taxon : null;
    }

    public Taxon? GetTaxonByDanishName(string danishName)
    {
        return lookupByVernacularName.TryGetValue(danishName, out Taxon? taxon) ? taxon : null;
    }

    public Taxon? GetTaxonParent(Taxon taxon)
    { // In doubt if this and next method should actually be here, and not in Taxon
        string parentId;
        
        if (taxon.SuperTaxon != null) parentId = taxon.SuperTaxon.TaxonID;
        else return null;
        
        return  GetTaxonById(parentId);
    }

    public List<Taxon>? GetTaxonChildren(Taxon taxon)
    {
        return taxon.SubTaxons.Count == 0 ? null : taxon.SubTaxons;
    }
}