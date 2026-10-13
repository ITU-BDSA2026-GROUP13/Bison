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
            .Include(o => o.Author);

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
            .Include(x => x.Author);
        
        var response = await query.ToListAsync();
        return response;
    }

    public async Task<List<Observation>> GetObservationsByAuthorAndPage(long authorId, int page)
    {
        int pageSize = 32;
        var query = (from observation in dbContext.Observations
                where observation.Author.AuthorId == authorId
                orderby observation.TimeStamp descending
                select observation)
            .Skip((Math.Max(page, 1) - 1) * pageSize)
            .Take(pageSize)
            .Include(x => x.Author);
        
        var response = await query.ToListAsync();
        return response;
    }

    public Task UpdateObservation(Observation observation)
    {
        throw new NotImplementedException();
    }
}