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

    public async Task AddProposal(ProposalDTO proposalDTO) // Change to ProposalCreateDTO when created
    {
        Proposal newProposal = new Proposal(
            // Create a proposal here. (Make ProposalCreateDTO aswell)
        );

        await unitOfWork.proposalRepo.CreateProposal(newProposal);
        unitOfWork.Save();
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
        if (author == null){throw new KeyNotFoundException("Author with id: " + authorId + " not found");}
        var taxon = await unitOfWork.taxonRepo.GetTaxonById(proposalCreateDTO.TaxonId);

        Proposal newProposal = new Proposal()
        {
            // Taxon related stuff
            Observation = observation,
            Taxon = taxon,

            // Post related stuff
            // PostId??? (probrably not needed here)
            Author = author,
            Text = taxon.VernacularName, // Do I get from TaxonOptions???
            TimeStamp = DateTime.Now
        };
    }

}