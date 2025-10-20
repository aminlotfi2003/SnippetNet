using SnippetNet.Domain.Identity;

namespace SnippetNet.Application.Identity.Dtos;

public sealed record ApplicationUserDto(
    Guid Id,
    string Email,
    string? FirstName,
    string? LastName,
    DateTimeOffset? BirthDate,
    bool IsActived,
    DateTimeOffset? LastPasswordChangedAt)
{
    public static ApplicationUserDto FromEntity(ApplicationUser user) =>
        new(
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName,
            user.BirthDate,
            user.IsActived,
            user.LastPasswordChangedAt
        );
}
