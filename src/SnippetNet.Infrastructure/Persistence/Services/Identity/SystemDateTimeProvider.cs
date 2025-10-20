using SnippetNet.Application.Common.Services.Identity;

namespace SnippetNet.Infrastructure.Persistence.Services.Identity;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
