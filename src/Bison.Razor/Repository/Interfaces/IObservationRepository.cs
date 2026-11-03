namespace Bison;

public interface IObservationRepository
{
    Task CreateObservation(Observation observation);
    
    Task<Observation?> ReadObservation(long observationId);

    Task<List<Observation>> GetObservationsByPage(int page);
    
    Task<List<Observation>> GetObservationsByAuthorAndPage(string authorId, int page);

    Task<List<Observation>> GetObservationByTaxonAndPage(string? taxon, int page);

    Task<List<Observation>> GetObservationByTaxonAndAuthorAndPage(string? taxonId, string authorId, int page);
    
    Task UpdateObservation(Observation observation);
}