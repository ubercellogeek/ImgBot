namespace ImgBot.Api.Services
{
    public class BackgroundQueueService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _queue;
        private readonly ILogger<BackgroundQueueService> _logger;

        public BackgroundQueueService(IBackgroundTaskQueue queue, ILogger<BackgroundQueueService> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           await _queue.RunAsync(stoppingToken);
        }
    }
}