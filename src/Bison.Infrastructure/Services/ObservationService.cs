namespace Bison.Services;

using Bison.DTOs;

using Interfaces;
using Razor.Services.Interfaces;
using Repository;
using System.Security.Claims;

public class ObservationService : IObservationService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IObservationFilterService filterService;
    private readonly IAuthorService authorService;
    private readonly ITaxonService taxonService;

    public ObservationService(IUnitOfWork unitOfWork, IObservationFilterService filterService, IAuthorService authorService, ITaxonService taxonService)
    {
        this.unitOfWork = unitOfWork;
        this.filterService = filterService;
        this.authorService = authorService;
        this.taxonService = taxonService;
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
        Taxon? root = await unitOfWork.taxonRepo.GetTaxonById(taxonId);
        List<Observation> all = await unitOfWork.observationRepo.GetAllObservations();

        return Translater(filterService.FilterAndPaginate(root, all, page));
    }

    // Same as above, but only for one author's observations
    public async Task<List<ObservationDTO>> GetObservationByTaxonAndAuthorAndPage(string? taxonId, string authorId, int page = 1)
    {
        if (taxonId == null) return await GetObservationsFromAuthorAndPage(authorId, page);
        Taxon? root = await unitOfWork.taxonRepo.GetTaxonById(taxonId);
        List<Observation> all = await unitOfWork.observationRepo.GetAllObservationsByAuthor(authorId);

        return Translater(filterService.FilterAndPaginate(root, all, page));
    }
    
    //The method for adding an observation to our databse
    public async Task CreateObservation(ObservationCreateDTO dto, string authorId)
    {
        var author = await unitOfWork.observationRepo.GetAuthorById(authorId)
                    ?? throw new ArgumentException("Unknown author", nameof(authorId));
        var taxon = await unitOfWork.observationRepo.GetTaxonFromId(dto.TaxonId)
                    ?? throw new ArgumentException("Unknown taxon", nameof(dto));

        var observation = new Observation
        {
            Author = author,
            Taxon = taxon,
            Text = dto.ObservationText,
            Location = "",                    
            TimeStamp = DateTime.UtcNow,      
            ImageFile = dto.ImageFile         
        };

        await unitOfWork.observationRepo.CreateObservation(observation);
        unitOfWork.Save();                    
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