namespace Service;

using Bison;
using SimpleDB;

public class ProposalService
{
    // Creates DBs
    private readonly IDatabaseRepository<Cheep> observations;
    private readonly IDatabaseRepository<Proposal> proposals;

    public ProposalService(IDatabaseRepository<Cheep> observations, IDatabaseRepository<Proposal> proposals)
    {
        this.observations = observations;
        this.proposals = proposals;
    }

    public void addProposal(Proposal proposal)
    {
        if (!CommentHandling.doesObservationExist(proposal.CheepID, observations.Read())) throw new InvalidOperationException("Observation id does not exist");
        proposals.Store(proposal);
    }

    public IEnumerable<Proposal> getProposals(long CheepID)
    {
        return proposals.Read().Where(p => p.CheepID == CheepID);
    }
}