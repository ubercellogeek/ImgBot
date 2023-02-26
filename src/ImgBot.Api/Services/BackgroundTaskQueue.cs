using System.Threading.Channels;
using ImgBot.Api.Configuration;
using Microsoft.Extensions.Options;

namespace ImgBot.Api.Services
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, Task>> _queue;
        private readonly ILogger<BackgroundTaskQueue> _logger;
        private readonly Task[] _consumers;

        public BackgroundTaskQueue(IOptions<ImgBotOptions> options, ILogger<BackgroundTaskQueue> logger)
        {
            _queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>(new());
            _logger = logger;
            _consumers = new Task[options.Value.WorkerCount];
        }

        public async Task EnqueueAsync(Func<CancellationToken, Task> workItem)
        {
            if(workItem is null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            await _queue.Writer.WriteAsync(workItem);

        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            for(int i = 0; i < _consumers.Length; i++)
            {
                _consumers[i] = Consumer(cancellationToken);
            }

            await Task.WhenAll(_consumers).ConfigureAwait(false);
        }

        private async Task Consumer(CancellationToken cancellationToken)
        {
            while (await _queue.Reader.WaitToReadAsync(cancellationToken))
            {
                try
                {
                    if(_queue.Reader.TryRead(out var item))
                    {
                        await item(cancellationToken).WaitAsync(cancellationToken).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception while running queued task.");
                }
            }
        }
    }
}