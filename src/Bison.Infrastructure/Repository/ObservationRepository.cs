namespace Bison.Repository;

using Microsoft.EntityFrameworkCore;

public class ObservationRepository : IObservationRepository
{

    private readonly BisonDBContext dbContext;

    public ObservationRepository(BisonDBContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task CreateObservation(Observation observation)
    {
        await dbContext.Posts.AddAsync(observation);
    }

    public async Task<Observation?> ReadObservation(long observationId)
    {
        var query = (from post in dbContext.Observations 
            where post.PostId == observationId
            select post)
            .Include(o => o.Author)
            .Include(o => o.Taxon);

        var response = await query.FirstOrDefaultAsync();
        return response;
    }

    public async Task<List<Observation>> GetObservationsByPage(int page)
    {
        int pageSize = 32;
        var query = (from observation in dbContext.Observations
                orderby observation.TimeStamp descending
                select observation)
            .Skip((Math.Max(page, 1) - 1) * pageSize)
            .Take(pageSize)
            .Include(x => x.Author)
            .Include(x => x.Taxon);
        
        var response = await query.ToListAsync();
        return response;
    }

    public async Task<List<Observation>> GetObservationsByAuthorAndPage(string authorId, int page)
    {
        int pageSize = 32;
        var query = (from observation in dbContext.Observations
                where observation.Author.Id == authorId
                orderby observation.TimeStamp descending
                select observation)
            .Skip((Math.Max(page, 1) - 1) * pageSize)
            .Take(pageSize)
            .Include(x => x.Author)
            .Include(x => x.Taxon);
        
        var response = await query.ToListAsync();
        return response;
    }

    // Fetches all observations (newest first) so the verified Dafny filter can run in memory
    public async Task<List<Observation>> GetAllObservations()
    {
        return await dbContext.Observations
            .OrderByDescending(o => o.TimeStamp)
            .Include(o => o.Author)
            .Include(o => o.Taxon)
            .ToListAsync();
    }

    // Same as above, limited to one author
    public async Task<List<Observation>> GetAllObservationsByAuthor(string authorId)
    {
        return await dbContext.Observations
            .Where(o => o.Author.Id == authorId)
            .OrderByDescending(o => o.TimeStamp)
            .Include(o => o.Author)
            .Include(o => o.Taxon)
            .ToListAsync();
    }

    // Loads the whole taxa table so EF fixes up the Parent navigation that isSubTaxon relies on
    public async Task<Taxon?> GetTaxonFromId(string taxonId)
    {
        var taxa = await dbContext.Taxons.ToListAsync();
        return taxa.FirstOrDefault(t => t.dwc_TaxonID == taxonId);
    }

    public Task UpdateObservation(Observation observation)
    {
        throw new NotImplementedException();
    }
}