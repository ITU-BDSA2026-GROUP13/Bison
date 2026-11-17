namespace Bison;

using DTOs;

public interface ITaxonRepository
{
    Task<Taxon?> GetTaxonById(string taxonId);

    Task<Taxon?> GetTaxonByVernacularName(string vernacularName);

    Task<List<Taxon>> GetAllTaxons();

    Task<List<Taxon>> GetAllTaxonsWithVernacularName();

 public Task<List<TaxonOptionDTO>> GetTaxonOptions();
}