namespace Bison;

public interface IProposalRepository
{
    Task CreateProposal(Proposal proposal);
    
    Task<List<Proposal>> ReadProposal(long proposalId);
    
    Task UpdateProposal(Proposal proposal);
}