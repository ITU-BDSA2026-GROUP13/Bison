namespace Bison.Tests.UnitTests;

using Bison.Services;

public class ObservationFilterServiceTests
{
    private readonly ObservationFilterService service = new();

    [Fact]
    public void Filter_IncludesRootAndChildrenAndIgnoresUnrelatedTaxons()
    {
        var root = CreateTaxon("root");
        var child = CreateTaxon("child", root);
        var unrelated = CreateTaxon("unrelated");
        var observations = new[]
        {
            CreateObservation(root),
            CreateObservation(unrelated),
            CreateObservation(child)
        };

        var result = service.Filter(root, observations);

        Assert.Equal(new[] { observations[0], observations[2] }, result);
    }

    [Fact]
    public void Filter_WithNullRootReturnsEmptyList()
    {
        var observations = new[]
        {
            CreateObservation(CreateTaxon("first")),
            CreateObservation(CreateTaxon("second"))
        };

        var result = service.Filter(null, observations);

        Assert.Empty(result);
    }

    [Fact]
    public void Filter_WithEmptyListReturnsEmptyList()
    {
        var result = service.Filter(CreateTaxon("root"), Array.Empty<Observation>());

        Assert.Empty(result);
    }

    [Fact]
    public void FilterAndPaginate_ReturnsRequestedPageOfFilteredObservations()
    {
        var root = CreateTaxon("root");
        var observations = new List<Observation>();
        for (int i = 0; i < 35; i++)
        {
            observations.Add(CreateObservation(root));   
        }

        var result = service.FilterAndPaginate(root, observations, 2);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void FilterAndPaginate_NegativePageNumberReturnsPage1()
    {
        var root = CreateTaxon("root");
        var observations = new List<Observation>();
        for (int i = 0; i < 35; i++)
        {
            observations.Add(CreateObservation(root));   
        }

        var result = service.FilterAndPaginate(root, observations, -3);

        Assert.Equal(32, result.Count);
    }


    private static Taxon CreateTaxon(string id, Taxon? parent = null) =>
        new()
        {
            dwc_TaxonID = id,
            Parent = parent
        };

    private static Observation CreateObservation(Taxon taxon) =>
        new()
        {
            Taxon = taxon,
            Location = "Test location",
            Author = null!,
            Text = "Test observation"
        };
}
