namespace Bison.Services;

using Dafny;

public class ObservationFilterService : IObservationFilterService
{
    private const int PageSize = 32;

    // Calls the verified Dafny filter: converts to a Dafny sequence, filters, converts back
    public List<Observation> Filter(Taxon? root, IEnumerable<Observation> observations)
    {
        // No taxon selected means no filtering
        if (root == null) return observations.ToList();

        var input = Sequence<Observation>.FromArray(observations.ToArray());
        var result = WrappedFilterBy.__default.WrappedFilter(root, input);
        return result.Elements.ToList();
    }

    // Filters with the verified Dafny code, then takes one page
    public List<Observation> FilterAndPaginate(Taxon? root, IEnumerable<Observation> observations, int page)
    {
        return Filter(root, observations)
            .Skip((Math.Max(page, 1) - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }
}