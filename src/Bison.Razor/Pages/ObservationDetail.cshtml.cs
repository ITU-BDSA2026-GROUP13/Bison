using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class ObservationDetailModel : PageModel
{
    readonly IObservationService _service;
    public int CurrentPage { get; set; }
    public ObservationViewModel? Observation { get; set; }
    public List<CommentViewModel>? Comments { get; set; }
    public List<ProposalViewModel>? Proposals { get; set; }

    public ObservationDetailModel(IObservationService service)
    {
        _service = service;
    }

    public ActionResult OnGet(long? id, [FromQuery] int page = 1)
    {
        if (id == null) return Redirect("/obs");
        this.CurrentPage = Math.Max(page, 1);
        this.Observation = _service.GetObservationFromId(id);
        if (Observation == null) return NotFound();
        this.Comments = _service.GetCommentViewModels(id);
        this.Proposals = _service.GetProposalViewModel(id);
        return Page();
    }
}