namespace SnippetNet.Application.Common.Services.Identity;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
