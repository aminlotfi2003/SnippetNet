namespace SnippetNet.Application.Identity.Dtos;

public sealed record ForgotPasswordResultDto(bool Success)
{
    public static ForgotPasswordResultDto Succeeded() => new(true);

    public static ForgotPasswordResultDto Failed() => new(false);
}
