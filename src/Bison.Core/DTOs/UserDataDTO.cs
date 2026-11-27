namespace Bison.DTOs;

public record UserDataDTO
(
    string? Id, 
    string? Username,
    string? NormalizedUsername,
    string? Email,
    string? NormalizedEmail,
    bool? EmailConfirmed,
    string? PasswordHash,
    string? SecurityStamp,
    string? ConcurrencyStamp,
    string? PhoneNumber,
    bool? PhoneNumberConfirmed,
    bool? TwoFactorEnabled,
    bool? LockoutEnabled,
    int? AccessFailedCount
);