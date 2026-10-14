namespace Bison.Services.Interfaces;

using Bison.DTOs;


public interface IProposalService
{
    public Task<List<ProposalDTO>> GetProposalsByObsId(long obsId);
}