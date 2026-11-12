namespace Bison.Razor.Pages;

using Bison.DTOs;
using Bison.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public abstract class ObservationFormPageModel : PageModel
{
    private readonly IObservationService observationService;
    private readonly ITaxonService taxonService;

    protected ObservationFormPageModel(IObservationService observationService, ITaxonService taxonService)
    {
        this.observationService = observationService;
        this.taxonService = taxonService;
    }

    [BindProperty] 
    public ObservationCreateDTO NewObservation { get; set; } = new();

    protected abstract Task LoadPageDataAsync();

    protected async Task LoadTaxonOptionsAsync()
    {
        var options = await taxonService.GetTaxonOptions();
        NewObservation.TaxonOptions = options
            .Select(o => new SelectListItem(o.VernacularName, o.dwc_TaxonId));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!(User.Identity?.IsAuthenticated ?? false)) // this weird construct beacuse the IsAuthenticated returns a bool? not a bool
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            await LoadTaxonOptionsAsync();
            await LoadPageDataAsync();
            return Page();
        }

        // TODO: IMPLEMENT: await observationService.CreateObservation(NewObservation, User);
        return RedirectToPage();
    }




}