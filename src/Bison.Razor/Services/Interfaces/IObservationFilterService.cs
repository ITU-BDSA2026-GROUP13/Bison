namespace Bison.Services;

public interface IObservationFilterService
{
    List<Observation> Filter(Taxon? root, IEnumerable<Observation> observations);
    List<Observation> FilterAndPaginate(Taxon? root, IEnumerable<Observation> observations, int page);
}