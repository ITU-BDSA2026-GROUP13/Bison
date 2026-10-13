using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

using Services.Interfaces;

public class UserTimelineModel : PageModel
{
    private readonly IObservationService _service;
    public int CurrentPage { get; set; }
    public List<Observation> Observations { get; set; }

    public UserTimelineModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet(long authorId, [FromQuery] int page = 1)
    {
        CurrentPage = Math.Max(page, 1);
        Observations = await _service.GetObservationsFromAuthorAndPage(authorId, page);
        return Page();
    }
}
