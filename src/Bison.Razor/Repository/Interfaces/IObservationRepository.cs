namespace Bison;

public interface IObservationRepository
{
    Task CreateObservation(Observation observation);
    
    Task<Observation?> ReadObservation(long observationId);

    Task<List<Observation>> GetObservationsByPage(int page);
    
    Task<List<Observation>> GetObservationsByAuthorAndPage(string authorId, int page);
    
    Task<List<Observation>> GetAllObservations();
    Task<List<Observation>> GetAllObservationsByAuthor(string authorId);
    
    Task UpdateObservation(Observation observation);
}