namespace ImgBot.Api.Services
{
    public interface IBackgroundTaskQueue
    {
        Task EnqueueAsync(Func<CancellationToken, Task> workItem);
        Task RunAsync(CancellationToken cancellationToken);
    }
}