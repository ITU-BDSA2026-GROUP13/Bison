using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class ObservationDetailModel : PageModel
{
    readonly IObservationService _serviceob;
    readonly ICommentService _serviceco;
    readonly IProposalService _servicepro;
    
    public int CurrentPage { get; set; }
    public ObservationViewModel? Observation { get; set; }
    public List<CommentViewModel>? Comments { get; set; }
    public List<ProposalViewModel>? Proposals { get; set; }

     public ObservationDetailModel(
          IObservationService serviceob,
          ICommentService serviceco,
          IProposalService servicepro)
    {
        _serviceob = serviceob;
        _serviceco = serviceco;
        _servicepro = servicepro;
    }

    public ActionResult OnGet(long? id, [FromQuery] int page = 1)
    {
        if (id == null) return Redirect("/obs");
        this.CurrentPage = Math.Max(page, 1);
        this.Observation = _serviceob.GetObservationFromId(id);
        if (Observation == null) return NotFound();
        this.Comments = _serviceco.GetCommentViewModels(id);
        this.Proposals = _servicepro.GetProposalViewModel(id);   
        return Page();
    }
}