namespace Bison.Razor.Pages;

using Bison.DTOs;
using Bison.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class StatsModel : PageModel
{
    private readonly IObservationService observationService;

    public StatsModel(IObservationService observationService)
    {
        this.observationService = observationService;
    }

    public List<TaxonStatDTO> Stats { get; private set; } = new();

    public async Task OnGetAsync()
    {
        Stats = await observationService.GetTaxonStats();
    }
}