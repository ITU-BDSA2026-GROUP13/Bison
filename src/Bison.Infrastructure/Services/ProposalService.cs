namespace Bison.Services;

using Bison.DTOs;
using Bison.Services.Interfaces;

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
            p.Author.Id,
            p.Taxon.dwc_TaxonID,
            p.Taxon.VernacularName ?? p.Taxon.dwc_TaxonID,
            p.Author.UserName ?? "",
            p.Text,
            p.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")
        )).ToList();
    }

    public async Task AddProposal(long observationId, string authorId, ProposalCreateDTO proposalCreateDTO)
    {
        var observation = await unitOfWork.observationRepo.ReadObservation(observationId);
        if (observation == null){throw new KeyNotFoundException("Observation with id: " + observationId + " not found");}
        
        var author = await unitOfWork.authorRepo.GetAuthorById(authorId);

        var taxon = await unitOfWork.taxonRepo.GetTaxonById(proposalCreateDTO.TaxonId);
        if (taxon == null){throw new KeyNotFoundException("Taxon with id: " + proposalCreateDTO.TaxonId + " not found");}

        Proposal newProposal = new Proposal()
        {
            Observation = observation,
            Taxon = taxon,
            Author = author,
            Text = taxon.VernacularName,
            TimeStamp = DateTime.Now
        };

        await unitOfWork.proposalRepo.CreateProposal(newProposal);
        unitOfWork.Save();
    }

}