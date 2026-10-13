namespace Bison.Services.Interfaces;

public interface IObservationService
{
    public Task<List<Observation>> GetObservations(int page);
    public Task<List<Observation>> GetObservationsFromAuthorAndPage(long authorId, int page = 1);
    
    public Task<Observation?> GetObservationById(long id);

    public Task<List<Observation>> GetObservationByTaxonAndPage(string? taxon, int page = 1);

    public Task<List<Observation>> GetObservationByTaxonAndAuthorAndPage(string? taxonId, long authorId,int page);
}