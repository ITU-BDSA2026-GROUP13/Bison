namespace Bison;

using CsvHelper;
using System.Globalization;
using System.IO;
using System.Reflection;

public class Taxonomy
{
    public List<Taxon> Taxons { get; set; } = new List<Taxon>();

    
    string csvEmbeddedResourcePath = "Bison.CLI.joined.csv";
    
    public void Taxonloader()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream(csvEmbeddedResourcePath);
        var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<TaxonMap>();
        Taxons = csv.GetRecords<Taxon>().ToList();
    }

    public void PrintTaxonomy()
    {
        foreach (var taxon in Taxons)
        {
            Console.WriteLine($"ID: {taxon.TaxonID}, Name: {taxon.VernacularName}, ParentID: {taxon.ParentNameUsageID}, SuperTaxon: {taxon.SuperTaxon}");
        
        }
    
    }
    public Taxon getTaxonByID(string id)
    {
        throw new NotImplementedException();
        //return Taxon;
    }

    public Taxon getTaxonByDanishName(string danishName)
    {
        throw new NotImplementedException();
        //return Taxon;
    }

    public Taxon getTaxonParentID(string parentID)
    {
        throw new NotImplementedException();
        //return Taxon;
    }

    public List<Taxon> getTaxonChildren(Taxon parent)
    {
        throw new NotImplementedException();
        //return Taxon;
    }
}