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
    public string? AuthorId { get; set; }
    public UserDataDTO? UserData { get; set; }
    public bool MyPage { get; set; }

    



    public AboutMe(IObservationService observationService, IUserDataService userDataService)
    {
        _userDataService = userDataService;
        _observationService = observationService;
    }
    public async Task OnGetAsync()
    {
        await LoadPersonalDataAsync();
        
    }

    protected async Task LoadPersonalDataAsync()
    {
        
        this.AuthorId = RouteData.Values["authorId"]?.ToString();

        UserData = await _userDataService.GetUserByIdAsync(AuthorId);

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (currentUserId != null && currentUserId == this.AuthorId)
        {
            this.MyPage = true;
        }
        else
        {
            this.MyPage = false;
        }
        
        if (UserData == null)
        {
            throw new Exception("Userdata is null");
        }
    }
    

    
    
}