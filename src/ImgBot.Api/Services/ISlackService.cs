using ImgBot.Api.Models.Slack;

namespace ImgBot.Api.Services
{
    public interface ISlackService
    {
        Task HandleSlashCommandAsync(string? text, string? respondUri, string? channelId, string? userId, string? teamId, string? enterpriseId, string? triggerId, CancellationToken cancellationToken);
        Task HandleActionAsync(string? baseUri, string? payload, CancellationToken cancellationToken);
        Task<TokenContext?> GetTokenContextAsync(string userId, string teamId, string? enterpriseId = null);
        Task SetTokenContextAsync(TokenContext context);
        Task DeleteMessageAsync(string replyUri, CancellationToken cancellationToken);
        Task SendPayload<T>(string uri, T payload, CancellationToken cancellationToken);
        Task SendMessageAsync(User user, TokenContext context, Message message, CancellationToken cancellationToken);
    }
}