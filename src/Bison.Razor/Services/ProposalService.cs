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

        return proposals.Select(p => new ProposalDTO
        {
            PostId = p.PostId,
            AuthorId = p.AuthorId,
            dwc_TaxonId = p.Taxon.dwc_TaxonId,
            TaxonName = p.Taxon.Name,
            AuthorName = p.Author.Name,
            Text = p.Text,
            Timestamp = p.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")
        }).ToList();
    }
}