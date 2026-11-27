namespace Bison.Services;

using Core.IServices;
using DTOs;

public class UserdataService : IUserDataService
    
{
    private readonly IUserDataRepository _repo;

    public UserdataService(IUserDataRepository repo)
    {
        _repo = repo;
    }

    public async Task<UserDataDTO?> GetUserByIdAsync(string userId)
    {
        var user = await _repo.GetByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        return new UserDataDTO(
            Id: user.Id,
            Username: user.UserName,
            NormalizedUsername: user.NormalizedUserName,
            Email: user.Email,
            NormalizedEmail: user.NormalizedEmail,
            EmailConfirmed: user.EmailConfirmed,
            PasswordHash: user.PasswordHash,
            SecurityStamp: user.SecurityStamp,
            ConcurrencyStamp: user.ConcurrencyStamp,
            PhoneNumber: user.PhoneNumber,
            PhoneNumberConfirmed: user.PhoneNumberConfirmed,
            TwoFactorEnabled: user.TwoFactorEnabled,
            LockoutEnabled: user.LockoutEnabled,
            AccessFailedCount: user.AccessFailedCount
        );
    }
}