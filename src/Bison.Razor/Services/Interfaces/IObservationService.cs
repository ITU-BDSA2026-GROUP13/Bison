namespace Bison.Services.Interfaces;

using Bison.DTOs;


public interface IObservationService
{
    public Task<List<ObservationDTO>> GetObservations(int page);
    public Task<List<ObservationDTO>> GetObservationsFromAuthorAndPage(long authorId, int page = 1);
    
    public Task<ObservationDTO?> GetObservationById(long id);

    public Task<List<ObservationDTO>> GetObservationByTaxonAndPage(string? taxon, int page = 1);

    public Task<List<ObservationDTO>> GetObservationByTaxonAndAuthorAndPage(string? taxonId, long authorId,int page);
}