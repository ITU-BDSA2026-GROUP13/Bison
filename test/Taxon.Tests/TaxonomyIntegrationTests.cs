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
    public void TaxonGetters_With_Wrong_Input()
    {
        //Arrange
        var taxonomy = GetTaxonomy();

        //Act
        var nonExistingIdTaxon = taxonomy.GetTaxonById("6767676767676767");
        var nonExistingNameTaxon = taxonomy.GetTaxonByDanishName("xXx!Lars!xXx");
        
        //Assert
        Assert.Null(nonExistingIdTaxon);
        Assert.Null(nonExistingNameTaxon);
    }
    
    [Fact]
    public void TaxonGetters_Reference_Same_Taxons()
    {
        //Arrange
        var taxonomy = GetTaxonomy();
        
        //Act
        var taxon = taxonomy.GetTaxonByDanishName("Årefodede"); // Pulling out example
        Assert.NotNull(taxon);
        var sameTaxon = taxonomy.GetTaxonById(taxon.TaxonID); // Extracting the same taxon through ID
        Assert.NotNull(sameTaxon);
        
        //Assert
        Assert.Same(sameTaxon, taxon);
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
        Assert.NotNull(actualParent);
        var taxonChild = taxonomy.GetTaxonByDanishName("Hejrer"); //Child
        Assert.NotNull(taxonChild);
        
        // Act
        var foundParent = taxonChild.SuperTaxon; //Relative parent
        Assert.NotNull(foundParent);
        var parentChildren = actualParent.SubTaxons; //Relative children
        
        //Assert
        Assert.Equal(actualParent.TaxonID, foundParent.TaxonID);
        Assert.Contains(taxonChild, parentChildren);
    }   
}