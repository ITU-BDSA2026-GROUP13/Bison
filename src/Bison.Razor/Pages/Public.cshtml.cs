using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

using Services.Interfaces;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    public List<Observation> Observations { get; set; }
    public int CurrentPage { get; set; }

    public PublicModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet([FromQuery] int page = 1)
    {
        CurrentPage = Math.Max(page, 1);
        Observations = await _service.GetObservations(page);
        return Page();
    }
}
