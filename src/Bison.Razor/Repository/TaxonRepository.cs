namespace Bison.Repository;

using Microsoft.EntityFrameworkCore;
using DTOs;

public class TaxonRepository : ITaxonRepository
{
    private readonly BisonDBContext dbContext;

    public TaxonRepository(BisonDBContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Taxon?> GetTaxonById(string taxonId)
    {
        var taxa = await dbContext.Taxons.ToListAsync();
        return taxa.FirstOrDefault(t => t.dwc_TaxonID == taxonId);
    }

    public Task<Taxon?> GetTaxonByVernacularName(string vernacularName)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Taxon>> GetAllTaxons()
    {
        return await dbContext.Taxons
            .OrderBy(t => t.TaxonId)
            .ToListAsync();
    }

    public async Task<List<Taxon>> GetAllTaxonsWithVernacularName()
    {
        return await dbContext.Taxons
            .Where(t => t.VernacularName != null)
            .OrderBy(t => t.VernacularName)
            .ToListAsync();
    }

    public async Task<List<TaxonOptionDTO>> GetTaxonOptions()
    {
        var taxons = await GetAllTaxonsWithVernacularName();
        List<TaxonOptionDTO> taxonOptionDtos = new List<TaxonOptionDTO>();

        foreach (Taxon t in taxons)
        {
            string dwc_TaxonId = t.dwc_TaxonID;
            string vernacularName = t.VernacularName;
            
            taxonOptionDtos.Add(new TaxonOptionDTO(dwc_TaxonId, vernacularName));
        }

        return taxonOptionDtos;
    }
}