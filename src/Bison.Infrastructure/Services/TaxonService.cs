namespace Bison.Services;

using DTOs;

using Interfaces;

public class TaxonService : ITaxonService
{
    readonly IUnitOfWork unitOfWork;

    public TaxonService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Taxon?> GetTaxonById(string taxonId)
    {
        return await unitOfWork.taxonRepo.GetTaxonById(taxonId);
    }

    public async Task<Taxon?> GetTaxonByVernacularName(string vernacularName)
    {
        return await unitOfWork.taxonRepo.GetTaxonByVernacularName(vernacularName);
    }

    public async Task<List<Taxon>> GetAllTaxons()
    {
        return await unitOfWork.taxonRepo.GetAllTaxons();
    }

    public async Task<List<Taxon>> GetAllTaxonsWithVernacularName()
    {
        return await unitOfWork.taxonRepo.GetAllTaxonsWithVernacularName();
    }

    public async Task<List<TaxonOptionDTO>> GetTaxonOptions()
    {
        return await unitOfWork.taxonRepo.GetTaxonOptions();
    }
    
    public async Task<List<TaxonStatDTO>> GetTaxonStats()
    {
        var counts = await unitOfWork.observationRepo.GetSightingCountsByTaxon();
        var taxa = await GetTaxonOptions();

        // Start from all taxa so birds with 0 sightings are included
        return taxa
            .Select(t => new TaxonStatDTO(
                t.dwc_TaxonId,
                t.VernacularName,
                counts.GetValueOrDefault(t.dwc_TaxonId)))
            .OrderByDescending(s => s.Count)
            .ThenBy(s => s.VernacularName)
            .ToList();
    }
}