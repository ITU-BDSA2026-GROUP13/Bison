namespace Bison.Services;

using Bison.DTOs;

using Interfaces;
using Repository;


public class ObservationService : IObservationService
{
    
    private readonly IUnitOfWork unitOfWork;

    public ObservationService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<List<ObservationDTO>> GetObservations(int page)
    {
        List<Observation> observations = await unitOfWork.observationRepo.GetObservationsByPage(page);
        return Translater(observations);
    }

    public async Task<List<ObservationDTO>> GetObservationsFromAuthorAndPage(long authorId, int page = 1)
    {
        List<Observation> observations = await unitOfWork.observationRepo.GetObservationsByAuthorAndPage(authorId, page);
        return Translater(observations);
    }

    public async Task<ObservationDTO?> GetObservationById(long id)
    {
        List<Observation> observations = await unitOfWork.observationRepo.ReadObservation(id);
        return Translater(observations);
    }

    public async Task<List<ObservationDTO>> GetObservationByTaxonAndPage(string? taxon, int page = 1)
    {
        List<Observation> observations = await unitOfWork.observationRepo.GetObservationByTaxonAndPage(taxon, page);
        return Translater(observations);
    }

    public async Task<List<ObservationDTO>> GetObservationByTaxonAndAuthorAndPage(string? taxonId, long authorId, int page = 1)
    {
        List<Observation> observations = await unitOfWork.observationRepo.GetObservationByTaxonAndAuthorAndPage(taxonId, authorId, page);
        return Translater(observations);
    }
    
    private Task<List<ObservationDTO>> Translater(List<Observation> observations)
    {
        return observations.Select(o => new CommentDTO
        {
            ObservationId = o.ObservationId,
            AuthorId = o.Author.AuthorId,
            dwc_TaxonId = o.Taxon.dwc_TaxonId,
            AuthorName = o.Author.Name,
            TaxonName = o.Taxon.Name,
            Text = o.Text,
            TimeStamp = 0.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")
        }).ToList();
    }
    
}