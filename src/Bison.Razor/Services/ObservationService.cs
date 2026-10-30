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

    public async Task<List<ObservationDTO>> GetObservationsFromAuthorAndPage(string authorId, int page = 1)
    {
        List<Observation> observations = await unitOfWork.observationRepo.GetObservationsByAuthorAndPage(authorId, page);
        return Translater(observations);
    }

    public async Task<ObservationDTO?> GetObservationById(long id)
    {
        Observation? observation = await unitOfWork.observationRepo.ReadObservation(id);
        return observation == null ? null : Translater(new List<Observation> { observation }).Single();
    }

    public async Task<List<ObservationDTO>> GetObservationByTaxonAndPage(string? taxon, int page = 1)
    {
        List<Observation> observations = await unitOfWork.observationRepo.GetObservationByTaxonAndPage(taxon, page);
        return Translater(observations);
    }

    public async Task<List<ObservationDTO>> GetObservationByTaxonAndAuthorAndPage(string? taxonId, string authorId, int page = 1)
    {
        List<Observation> observations = await unitOfWork.observationRepo.GetObservationByTaxonAndAuthorAndPage(taxonId, authorId, page);
        return Translater(observations);
    }
    
    private List<ObservationDTO> Translater(List<Observation> observations)
    {
        return observations.Select(o => new ObservationDTO(
            o.PostId,
            o.Author.Id,
            o.Taxon.dwc_TaxonID,
            o.Author.UserName ?? "",
            o.Taxon.VernacularName ?? o.Taxon.dwc_TaxonID,
            o.Text,
            o.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")
        )).ToList();
    }
    
}