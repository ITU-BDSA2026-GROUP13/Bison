namespace Bison.Repository;

using Microsoft.EntityFrameworkCore;

public class ProposalRepository : IProposalRepository
{
    
    private readonly BisonDBContext dbContext;

    public ProposalRepository(BisonDBContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    
    public async Task CreateProposal(Proposal proposal)
    {
        await dbContext.Posts.AddAsync(proposal);
    }

    public async Task<List<Proposal>> ReadProposal(long postId)
    {
        var query = from post in dbContext.Proposals
            where post.PostId == postId
            select post;

        var response = await query.ToListAsync();
        return response;
    }

    public Task<List<Proposal>> GetProposalsByObsId(long obsId)
    {
        var query = (from post in dbContext.Proposals
            where post.Observation.PostId == obsId
            select post)
            .Include(p => p.Author)
            .Include(p => p.Taxon);

        var response = query.ToListAsync();
        return response;
    }

    public Task UpdateProposal(Proposal proposal)
    {
        throw new NotImplementedException();
    }
}