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
    public int Total { get; private set; }

    public async Task OnGetAsync()
    {
        Stats = await observationService.GetTaxonStats();
        Total = Stats.Sum(s => s.Count);
    }

    public double Percent(int count)
    {
        return Total == 0 ? 0 : count * 100.0 / Total;
    }
}