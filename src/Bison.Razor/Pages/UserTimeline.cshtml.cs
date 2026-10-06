using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IObservationService _service;
    public int CurrentPage { get; set; }
    public List<ObservationViewModel> Observations { get; set; }

    public UserTimelineModel(IObservationService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author, [FromQuery] int page = 1)
    {
        CurrentPage = Math.Max(page, 1);
        Observations = _service.GetObservationsFromAuthor(author, page);
        return Page();
    }
}
