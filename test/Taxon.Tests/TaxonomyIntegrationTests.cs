namespace Bison.Tests;

using Xunit;
using Bison;
    
public class TaxonomyIntegrationTests
{

    private static Taxonomy GetTaxonomy()
    {
        Taxonomy taxonomy = new Taxonomy();
        taxonomy.Taxonloader();
        return taxonomy;
    }

    
    [Fact]
    public void TaxonsLoader_Loads()
    {
        //Arrange & Act start
        var taxonomy = GetTaxonomy(); // Calls Taxonloader

        var taxon = taxonomy.GetTaxonByDanishName("Årefodede"); // Pulling out example
        
        //Assert
        Assert.NotNull(taxon);
    }
    
    
    [Fact]
    public void Taxonomy_Prints()
    {
        //Arrange
        var taxonomy = GetTaxonomy(); // Calls Taxonloader
        
        using (StringWriter sw = new StringWriter())
        {

            TextWriter originalOutput = Console.Out;
            Console.SetOut(sw);
            
            // Act
            taxonomy.PrintTaxonomy();
            
            //Assert
            Assert.Contains("ID:", sw.ToString());
            
            Console.SetOut(originalOutput);
        }
    }

    [Fact]
    public void TaxonomyHierarchy_Exists_IE_ParentChild()
    {
        //Arrange
        var taxonomy = GetTaxonomy();
        
        var actualParent = taxonomy.GetTaxonByDanishName("Årefodede"); //Parent 
        var taxonChild = taxonomy.GetTaxonByDanishName("Hejrer"); //Child
        
        // Act
        var foundParent = taxonomy.GetTaxonParent(taxonChild); //Relative parent
        var parentChildren = actualParent.SubTaxons; //Relative children
        
        //Assert
        Assert.Equal(actualParent.TaxonID, foundParent.TaxonID);
        Assert.Contains(taxonChild, parentChildren);
    }
}