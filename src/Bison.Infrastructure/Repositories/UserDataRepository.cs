namespace Bison.Repository;

using Microsoft.AspNetCore.Identity;

public class UserDataRepository : IUserDataRepository
{
    private readonly UserManager<Author> _userManager;

    public UserDataRepository(UserManager<Author> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Author?> GetByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }
}

