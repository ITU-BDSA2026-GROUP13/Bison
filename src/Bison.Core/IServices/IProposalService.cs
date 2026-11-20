namespace Bison.Services.Interfaces;

using Bison.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProposalService
{
    public Task AddProposal(ProposalDTO proposalDTO);
    public Task<List<ProposalDTO>> GetProposalsByObsId(long obsId);
}