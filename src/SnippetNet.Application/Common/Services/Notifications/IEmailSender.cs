namespace SnippetNet.Application.Common.Services.Notifications;

public interface IEmailSender
{
    Task SendPasswordResetCodeAsync(string email, string code, CancellationToken cancellationToken = default);
}
