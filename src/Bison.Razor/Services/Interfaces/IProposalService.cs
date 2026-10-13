namespace Bison.Services.Interfaces;

public interface IProposalService
{
    public Task<List<Proposal>> GetProposalsByObsId(long obsId);
}