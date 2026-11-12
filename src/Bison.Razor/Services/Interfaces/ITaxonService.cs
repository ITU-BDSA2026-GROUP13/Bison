namespace Bison.Services.Interfaces;

using DTOs;

public interface ITaxonService
{
    public Task<Taxon?> GetTaxonById(string taxonId);
    
    public Task<Taxon?> GetTaxonByVernacularName(string vernacularName);
    
    public Task<List<Taxon>> GetAllTaxons();

    public Task<List<Taxon>> GetAllTaxonsWithVernacularName();

    public Task<List<TaxonOptionDTO>> GetTaxonOptions();
}