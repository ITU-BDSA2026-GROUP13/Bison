namespace Bison.Services.Interfaces;

public interface IProposalService
{
    public List<Proposal> GetProposalById(long? proposalId);
}