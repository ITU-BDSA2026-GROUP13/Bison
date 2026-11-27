namespace Bison.Razor.Pages;

using Bison.DTOs;
using Bison.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

public abstract class ObservationFormPageModel : PageModel
{
    private readonly IObservationService observationService;
    private readonly ITaxonService taxonService;

    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
    private const long MaxImageBytes = 15 * 1024 * 1024;

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
    if (!(User.Identity?.IsAuthenticated ?? false))
    {
        return Forbid();
    }

    bool isLoaded = false;

    if (NewObservation.Image is { Length: > 0 })
    {
        var ext = Path.GetExtension(NewObservation.Image.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(ext) || NewObservation.Image.Length > MaxImageBytes)
        {
            ModelState.AddModelError("NewObservation.Image", "Only jpg or png, max 15 MB.");
        }

        if (!ModelState.IsValid)
        {
            isLoaded = true;
            await LoadTaxonOptionsAsync();
            await LoadPageDataAsync();
            return Page();
        }
        var fileName = $"{Guid.NewGuid()}{ext}";   // never reuse the client's file name

        var env = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
        var dir = Path.Combine(env.WebRootPath, "images");
        Directory.CreateDirectory(dir);

        await using var stream = System.IO.File.Create(Path.Combine(dir, fileName));
        await NewObservation.Image.CopyToAsync(stream);

        NewObservation.ImageFile = fileName;
    }
    else
    {
        if (!ModelState.IsValid && isLoaded)
        {
            isLoaded = false;
            await LoadTaxonOptionsAsync();
            await LoadPageDataAsync();
            return Page();
        }
    }

    var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    await observationService.CreateObservation(NewObservation, authorId);
    return RedirectToPage();
}
}
