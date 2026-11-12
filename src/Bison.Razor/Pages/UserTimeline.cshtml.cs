using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

using Bison.DTOs;
using Services.Interfaces;

public class UserTimelineModel : ObservationFormPageModel
{
    private readonly IObservationService _service;
    public int CurrentPage { get; set; }
    public List<ObservationDTO> Observations { get; set; } = new();
    public string? FilteredTaxon { get; set; }


    public UserTimelineModel(IObservationService service, ITaxonService taxonService) : base(service, taxonService)
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
        var authorId = RouteData.Values["authorId"]?.ToString();

        int.TryParse(Request.Query["page"], out var page);
        CurrentPage = Math.Max(page, 1);

        string taxonId = Request.Query["taxonId"];
        FilteredTaxon = taxonId;

        Observations = await _service.GetObservationByTaxonAndAuthorAndPage(taxonId, authorId, page);
    }
}
