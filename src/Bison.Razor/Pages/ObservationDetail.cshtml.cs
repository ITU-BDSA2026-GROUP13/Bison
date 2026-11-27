using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

using Bison.DTOs;
using Bison.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

public class ObservationDetailModel : PageModel
{
    readonly IObservationService _serviceob;
    readonly ICommentService _serviceco;
    readonly IProposalService _servicepro;
    private readonly ITaxonService _taxonService;
    
    
    public int CurrentPage { get; set; }
    public ObservationDTO? Observation { get; set; }
    public List<CommentDTO> Comments { get; set; } = new();
    public List<ProposalDTO> Proposals { get; set; } = new();


    public CommentCreateDTO NewComment { get; set; } = new();

    public ProposalCreateDTO NewProposal { get; set; } = new();

    protected async Task LoadTaxonOptionsAsync()
    {
        var options = await _taxonService.GetTaxonOptions();
        NewProposal.TaxonOptions = options
            .Select(o => new SelectListItem(o.VernacularName, o.dwc_TaxonId));
    }

     public ObservationDetailModel(
          IObservationService serviceob,
          ICommentService serviceco,
          IProposalService servicepro,
          ITaxonService taxonService)
    {
        _serviceob = serviceob;
        _serviceco = serviceco;
        _servicepro = servicepro;
        _taxonService = taxonService;
    }

    public async Task<ActionResult> OnGet(long? id, [FromQuery] int page = 1)
    {
        if (id == null) return Redirect("/obs");
        this.CurrentPage = Math.Max(page, 1);
        this.Observation = await _serviceob.GetObservationById(id.Value);
        if (Observation == null) return NotFound();
        this.Comments = await _serviceco.GetCommentsByObsId(id.Value);
        this.Proposals = await _servicepro.GetProposalsByObsId(id.Value);   

        await LoadTaxonOptionsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostComment(long id, [Bind(Prefix = "NewComment")] CommentCreateDTO comment)
    { 
        if (!(User.Identity?.IsAuthenticated ?? false)) // this weird construct beacuse the IsAuthenticated returns a bool? not a bool
        {
            return Forbid();
        }

        ModelState.Remove("NewProposal.TaxonId");

        if (!ModelState.IsValid)
        {
            await LoadTaxonOptionsAsync();
            return RedirectToPage(new { id });
        }

        var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(authorId)) return Forbid();

        await _serviceco.AddComment(id, authorId, comment);
        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostProposal(long id, [Bind(Prefix = "NewProposal")] ProposalCreateDTO proposal)
    {
        if (!(User.Identity?.IsAuthenticated ?? false)) // this weird construct beacuse the IsAuthenticated returns a bool? not a bool
        {
            return Forbid();
        }

        ModelState.Remove("NewComment.Text");
        if (!ModelState.IsValid)
        {
            await LoadTaxonOptionsAsync();
            return RedirectToPage(new { id });
        }

        var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(authorId)) return Forbid();


        await _servicepro.AddProposal(id, authorId, proposal);
        return RedirectToPage(new { id });
    }
}