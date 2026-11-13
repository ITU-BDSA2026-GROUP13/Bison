using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

using Bison.DTOs;
using Services.Interfaces;

public class PublicModel : ObservationFormPageModel
{
    private readonly IObservationService _service;
    public List<ObservationDTO> Observations { get; set; } = new();
    public int CurrentPage { get; set; }
    public string? FilteredTaxon { get; set;  }

    public PublicModel(IObservationService service, ITaxonService taxonService) : base(service, taxonService)
    {
        _service = service;
    }

    public async Task OnGetAsync()
    {
        await LoadTaxonOptionsAsync();
        await LoadPageDataAsync();
    }

    protected override async Task LoadPageDataAsync()
    {
        int.TryParse(Request.Query["page"], out var page);
        CurrentPage = Math.Max(page, 1);

        string taxonId = Request.Query["taxonId"];
        if (taxonId == "")
        {
            FilteredTaxon = null;
        }
        else
        {
            this.FilteredTaxon = taxonId; 
        }

        Observations = await _service.GetObservationByTaxonAndPage(this.FilteredTaxon, page);
    }
}
