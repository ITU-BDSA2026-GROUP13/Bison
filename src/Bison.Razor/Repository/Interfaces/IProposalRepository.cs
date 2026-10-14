namespace Bison;

public interface IProposalRepository
{
    Task CreateProposal(Proposal proposal);
    
    Task<List<Proposal>> ReadProposal(long proposalId);

    Task<List<Proposal>> GetProposalsByObsId(long obsId);
    
    Task UpdateProposal(Proposal proposal);
}