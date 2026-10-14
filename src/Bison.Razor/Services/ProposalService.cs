namespace Bison.Services;

using Bison.DTOs;


using Interfaces;

public class ProposalService : IProposalService
{
    readonly IUnitOfWork unitOfWork;

    public ProposalService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<List<ProposalDTO>> GetProposalsByObsId(long postId)
    {
        List<Proposal> proposals = await unitOfWork.proposalRepo.GetProposalsByObsId(postId);

        return proposals.Select(p => new ProposalDTO(
            p.PostId,
            p.Author.AuthorId,
            p.Taxon.dwc_TaxonID,
            p.Taxon.VernacularName ?? p.Taxon.dwc_TaxonID,
            p.Author.Name,
            p.Text,
            p.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")
        )).ToList();
    }
}