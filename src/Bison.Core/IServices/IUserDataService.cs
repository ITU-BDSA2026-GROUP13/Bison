namespace Bison.Core.IServices;

using DTOs;

public interface IUserDataService
{
    Task<UserDataDTO?> GetUserByIdAsync(string userId);

}