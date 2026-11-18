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

        //should maybe be in the constructor
        var env = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
        var dir = Path.Combine(env.WebRootPath, "images");
        Directory.CreateDirectory(dir);

        if (!(User.Identity?.IsAuthenticated ?? false)) // this weird construct beacuse the IsAuthenticated returns a bool? not a bool
        {
            return Forbid();
        }
        
        if (NewObservation.Image is { Length: > 0 })
        {
            var ext = Path.GetExtension(NewObservation.Image.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext) || NewObservation.Image.Length > MaxImageBytes)
            {
                ModelState.AddModelError("NewObservation.Image", "Only jpg or png, max 15 MB.");
            }
        }
        
        if (!ModelState.IsValid)
        {
            await LoadTaxonOptionsAsync();
            await LoadPageDataAsync();
            return Page();
        }

        //  save the file
        if (NewObservation.Image is { Length: > 0 })
        {
            var ext = Path.GetExtension(NewObservation.Image.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{ext}";          // never reuse the client's file name
            dir = Path.Combine(env.WebRootPath, "images");
            Directory.CreateDirectory(dir);

            await using var stream = System.IO.File.Create(Path.Combine(dir, fileName));
            await NewObservation.Image.CopyToAsync(stream);

            NewObservation.ImageFile = fileName;
        }

        var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;   // using System.Security.Claims;
        await observationService.CreateObservation(NewObservation, authorId);
        return RedirectToPage();
    }
}
