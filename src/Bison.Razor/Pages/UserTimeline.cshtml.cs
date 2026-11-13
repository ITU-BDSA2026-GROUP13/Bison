using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

using Bison.DTOs;
using Services.Interfaces;
using System.Security.Claims;

public class UserTimelineModel : ObservationFormPageModel
{
    private readonly IObservationService _service;
    public int CurrentPage { get; set; }
    public List<ObservationDTO> Observations { get; set; } = new();
    public string? FilteredTaxon { get; set; }
    public string? AuthorId { get; set; }
    public bool MyPage { get; set; }


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
        this.AuthorId = RouteData.Values["authorId"]?.ToString();
        

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

        this.MyPage = User.FindFirstValue(ClaimTypes.NameIdentifier).Equals(this.AuthorId);

        Observations = await _service.GetObservationByTaxonAndAuthorAndPage(this.FilteredTaxon, this.AuthorId, this.CurrentPage);
    }
}
