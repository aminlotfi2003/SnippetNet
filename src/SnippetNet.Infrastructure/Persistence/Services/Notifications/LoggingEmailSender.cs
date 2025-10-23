using Microsoft.Extensions.Logging;
using SnippetNet.Application.Common.Services.Notifications;

namespace SnippetNet.Infrastructure.Persistence.Services.Notifications;

public sealed class LoggingEmailSender(ILogger<LoggingEmailSender> logger) : IEmailSender
{
    private readonly ILogger<LoggingEmailSender> _logger = logger;

    public Task SendPasswordResetCodeAsync(string email, string code, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending password reset code {Code} to {Email}", code, email);
        return Task.CompletedTask;
    }
}
