namespace SnippetNet.Application.Identity.Dtos;

public sealed record AuthenticationResultDto(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt)
{
    public static AuthenticationResultDto FromTokenPair(TokenPair tokenPair) =>
        new(
            tokenPair.AccessToken,
            tokenPair.AccessTokenExpiresAt,
            tokenPair.RefreshToken,
            tokenPair.RefreshTokenExpiresAt
        );
}
