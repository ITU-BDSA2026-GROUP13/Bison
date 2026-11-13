namespace Bison.Services;

using Bison.DTOs;

using Interfaces;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Repository;
using System.Security.Claims;

public class ObservationService : IObservationService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IObservationFilterService filterService;

    public ObservationService(IUnitOfWork unitOfWork, IObservationFilterService filterService)
    {
        this.unitOfWork = unitOfWork;
        this.filterService = filterService;
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

    // Filters by taxon and its subtaxa using the verified Dafny code
    public async Task<List<ObservationDTO>> GetObservationByTaxonAndPage(string? taxonId, int page = 1)
    {
        if (taxonId == null) return await GetObservations(page);
        Taxon? root = await unitOfWork.observationRepo.GetTaxonFromId(taxonId);
        List<Observation> all = await unitOfWork.observationRepo.GetAllObservations();

        return Translater(filterService.FilterAndPaginate(root, all, page));
    }

    // Same as above, but only for one author's observations
    public async Task<List<ObservationDTO>> GetObservationByTaxonAndAuthorAndPage(string? taxonId, string authorId, int page = 1)
    {
        if (taxonId == null) return await GetObservationsFromAuthorAndPage(authorId, page);
        Taxon? root = await unitOfWork.observationRepo.GetTaxonFromId(taxonId);
        List<Observation> all = await unitOfWork.observationRepo.GetAllObservationsByAuthor(authorId);

        return Translater(filterService.FilterAndPaginate(root, all, page));
    }
    
    //The method for adding an observation to our databse
    public async Task CreateObservation(ObservationCreateDTO NewObservation, ClaimsPrincipal user)
    {
        var observationToAdd = new Observation()
        { 
            //To do get the auther from id exposed in claims and add it to the observation object 
            //Author = user.FindFirstValue(ClaimTypes.NameIdentifier), Taxon = tArdeaCinerea, Text = "Grey heron standing still in the reeds at the lake.",
            TimeStamp = DateTime.Parse("2023-08-01 07:12:00")
        };

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
            o.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"),
            o.ImageFile
            )).ToList();
    }
}