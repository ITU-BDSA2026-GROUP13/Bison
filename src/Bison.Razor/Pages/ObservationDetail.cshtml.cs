using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

using Bison.DTOs;
using Services.Interfaces;

public class ObservationDetailModel : PageModel
{
    readonly IObservationService _serviceob;
    readonly ICommentService _serviceco;
    readonly IProposalService _servicepro;
    
    public int CurrentPage { get; set; }
    public ObservationDTO? Observation { get; set; }
    public List<CommentDTO> Comments { get; set; } = new();
    public List<ProposalDTO> Proposals { get; set; } = new();

     public ObservationDetailModel(
          IObservationService serviceob,
          ICommentService serviceco,
          IProposalService servicepro)
    {
        _serviceob = serviceob;
        _serviceco = serviceco;
        _servicepro = servicepro;
    }

    public async Task<ActionResult> OnGet(long? id, [FromQuery] int page = 1)
    {
        if (id == null) return Redirect("/obs");
        this.CurrentPage = Math.Max(page, 1);
        this.Observation = await _serviceob.GetObservationById(id.Value);
        if (Observation == null) return NotFound();
        this.Comments = await _serviceco.GetCommentsByObsId(id.Value);
        this.Proposals = await _servicepro.GetProposalsByObsId(id.Value);   
        return Page();
    }
}