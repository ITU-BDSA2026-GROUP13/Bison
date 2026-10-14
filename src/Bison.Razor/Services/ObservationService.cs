namespace Bison.Services;

using Interfaces;
using Repository;


public class ObservationService : IObservationService
{
    
    private readonly IUnitOfWork unitOfWork;

    public ObservationService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<List<Observation>> GetObservations(int page)
    {
        return await unitOfWork.observationRepo.GetObservationsByPage(page);
    }

    public Task<List<Observation>> GetObservationsFromAuthorAndPage(long authorId, int page = 1)
    {
        return unitOfWork.observationRepo.GetObservationsByAuthorAndPage(authorId, page);
    }

    public Task<Observation?> GetObservationById(long id)
    {
        return unitOfWork.observationRepo.ReadObservation(id);
    }

    public Task<List<Observation>> GetObservationByTaxonAndPage(string? taxon, int page = 1)
    {
        return unitOfWork.observationRepo.GetObservationByTaxonAndPage(taxon, page);
    }

    public Task<List<Observation>> GetObservationByTaxonAndAuthorAndPage(string? taxonId, long authorId, int page = 1)
    {
        return unitOfWork.observationRepo.GetObservationByTaxonAndAuthorAndPage(taxonId, authorId, page);
    }
    
    
}