using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

using Bison.DTOs;
using Services.Interfaces;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationDTO> Observations { get; set; } = new();
    public int CurrentPage { get; set; }
    public string? FilteredTaxon { get; set;  }

    public PublicModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet([FromQuery] int page = 1, string? taxonId = null)
    {
        CurrentPage = Math.Max(page, 1);
        FilteredTaxon = taxonId;
        Observations = await _service.GetObservationByTaxonAndPage(taxonId,page);
        return Page();
    }
}
