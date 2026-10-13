namespace Bison;

public interface IObservationRepository
{
    Task CreateObservation(Observation observation);
    
    Task<List<Observation>> ReadObservation(long observationId);

    Task<List<Observation>> GetObservationsByPage(int page);
    
    Task<List<Observation>> GetObservationsByAuthorAndPage(long authorId, int page);
    
    Task UpdateObservation(Observation observation);
}