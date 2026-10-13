namespace Bison.Services;

using Interfaces;

public class ProposalService : IProposalService
{
    readonly IUnitOfWork unitOfWork;

    public ProposalService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<List<Proposal>> GetProposalsByObsId(long obsId)
    {
        return await unitOfWork.proposalRepo.GetProposalsByObsId(obsId);
    }
}