using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

using Bison.Services.Interfaces;
using Core.IServices;
using DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public class AboutMe : PageModel
{
    private readonly IUserDataService _userDataService;
    private readonly IObservationService _observationService;
    private readonly ICommentService _commentService;
    private readonly IProposalService _proposalService;


    public string? AuthorId { get; set; }
    public UserDataDTO? UserData { get; set; }
    public List<ObservationDTO> ObservationData { get; set; }
    public List<CommentDTO> CommentData { get; set; }
    public List<ProposalDTO> ProposalData { get; set; }

    public bool MyPage { get; set; }

    



    public AboutMe(IObservationService observationService, IUserDataService userDataService, ICommentService commentService, IProposalService proposalService)
    {
        _userDataService = userDataService;
        _observationService = observationService;
        _commentService = commentService;
        _proposalService = proposalService;
    }
    public async Task OnGetAsync()
    {
        await LoadPersonalDataAsync();
        await LoadPersonalObservations();
        await LoadPersonalComments();
        await LoadPersonalProposals();


    }

    protected async Task LoadPersonalDataAsync()
    {
        
        this.AuthorId = this.RouteData.Values["authorId"]?.ToString();

        this.UserData = await _userDataService.GetUserByIdAsync(AuthorId);

        var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (currentUserId != null && currentUserId == this.AuthorId)
        {
            this.MyPage = true;
        }
        else
        {
            this.MyPage = false;
        }
        
        if (this.UserData == null)
        {
            throw new Exception("Userdata is null");
        }
    }

    protected async Task LoadPersonalObservations()
    {
        this.AuthorId = this.RouteData.Values["authorId"]?.ToString();
        this.ObservationData = await _observationService.GetObservationByAuthor(AuthorId);
    }

    protected async Task LoadPersonalComments()
    {
        this.AuthorId = this.RouteData.Values["authorId"]?.ToString();
        this.CommentData = await _commentService.GetCommentsByAuthor(AuthorId);
    }

    protected async Task LoadPersonalProposals()
    {
        this.AuthorId = this.RouteData.Values["authorId"]?.ToString();
        this.ProposalData = await _proposalService.GetProposalsByAuthor(AuthorId);
    }

    

    
    
}