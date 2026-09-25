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
        //Assert & Act start
        var taxonomy = GetTaxonomy(); // Calls Taxonloader
        
        using (StringWriter sw = new StringWriter())
        {

            TextWriter originalOutput = Console.Out;
            Console.SetOut(sw);
            
            taxonomy.PrintTaxonomy();
            
            //Assert
            Assert.Contains("ID:", sw.ToString());
            
            Console.SetOut(originalOutput);
        }
    }
    
    
    
    
}