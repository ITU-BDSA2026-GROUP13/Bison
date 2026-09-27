namespace Bison.Tests;

using Xunit;
using Bison;

public class TaxonomyUnitTest
{
    private static Taxonomy BuildSampleTaxonomy()
    {
        var margrethe = new Taxon { TaxonID = "1", VernacularName = "Margrethe", ParentNameUsageID = "" };
        var frederik  = new Taxon { TaxonID = "2", VernacularName = "Frederik",  ParentNameUsageID = "1" };
        var christian = new Taxon { TaxonID = "3", VernacularName = "Christian", ParentNameUsageID = "2" };
        var isabella  = new Taxon { TaxonID = "4", VernacularName = "Isabella",  ParentNameUsageID = "2" };

        var taxonomy = new Taxonomy();
        taxonomy.BuildStructures(new List<Taxon> { margrethe, frederik, christian, isabella });
        return taxonomy;
    }

    [Fact]
    public void ReturnsCorrectTaxon()
    {
        var taxonomy = BuildSampleTaxonomy();
        var frederik = taxonomy.GetTaxonById("2");

        Assert.NotNull(frederik);
        Assert.Equal("2", frederik!.TaxonID);
    }

    [Fact]
    public void IdDoesNotExist()
    {
        var taxonomy = BuildSampleTaxonomy();

        Assert.Null(taxonomy.GetTaxonById("999"));
    }

    [Fact]
    public void GetTaxonByDanishName()
    {
        var taxonomy = BuildSampleTaxonomy();
        var christian = taxonomy.GetTaxonByDanishName("Christian");

        Assert.NotNull(christian);
        Assert.Equal("3", christian!.TaxonID);
    }

    [Fact]
    public void GetCorrectTaxonParent()
    {
        var taxonomy = BuildSampleTaxonomy();
        var margrethe = taxonomy.GetTaxonById("1");

        Assert.NotNull(margrethe);
        var parent = margrethe.SuperTaxon;

        Assert.Null(parent);
    }

    [Fact]
    public void GetTaxonParent_ReturnsCorrectParent_ForNonRootTaxon()
    {
        //Arrange
        var taxonomy = BuildSampleTaxonomy();
        
        //Act
        var christian = taxonomy.GetTaxonById("3");
        var isabella = taxonomy.GetTaxonById("4");
        Assert.NotNull(christian);
        Assert.NotNull(isabella);
        var parent1 = christian.SuperTaxon;
        var parent2 = isabella.SuperTaxon;
        Assert.NotNull(parent1);
        Assert.NotNull(parent2);
        
        //Assert
        Assert.Equal("2", parent1!.TaxonID);
        Assert.Equal("2", parent2!.TaxonID);
    }

    [Fact]
    public void GetTaxonChildren_ReturnsEmptyList_ForLeafTaxon()
    {
        //Arrange
        var taxonomy = BuildSampleTaxonomy();
        var christian = taxonomy.GetTaxonById("3");
        var isabella = taxonomy.GetTaxonById("4");
        Assert.NotNull(christian);
        Assert.NotNull(isabella);

        //Act
        var children1 = christian.SubTaxons;
        var children2 = christian.SubTaxons;

        //Assert
        Assert.Empty(children1);
        Assert.Empty(children2);
    }

    [Fact]
    public void GetTaxonChildren_ReturnsChildren_ForParentTaxon()
    {
        //Arrange
        var taxonomy = BuildSampleTaxonomy();
        var frederik = taxonomy.GetTaxonById("2");
        Assert.NotNull(frederik);
        
        //Act
        var children = frederik.SubTaxons;

        //Assert
        Assert.NotNull(children);
        Assert.Equal(2, children!.Count);
    }
}