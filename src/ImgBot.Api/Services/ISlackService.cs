namespace ImgBot.Api.Services
{
    public interface ISlackService
    {
        Task HandleSlashCommandAsync(string? text, string? respondUri, string? channelId, string? userId, string? triggerId, CancellationToken cancellationToken);
        Task HandleActionAsync(string? baseUri, string? payload, CancellationToken cancellationToken);
    }
}