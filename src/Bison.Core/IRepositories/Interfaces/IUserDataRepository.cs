namespace Bison;

using Microsoft.AspNetCore.Identity;

public interface IUserDataRepository
{
    Task<Author?> GetByIdAsync(string userId);
}