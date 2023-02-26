using ImgBot.Api.Authorization;
using ImgBot.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImgBot.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SlackController : ControllerBase
    {

        private readonly IBackgroundTaskQueue _queue;
        private readonly ISlackService _slackService;

        public SlackController(IBackgroundTaskQueue queue, ISlackService slackService)
        {
            _queue = queue;
            _slackService = slackService;
        }

        [HttpPost("")]
        [SlackAuthorize]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> IncomingWebhook(
            [FromForm] string? token,
            [FromForm] string? command,
            [FromForm] string? text,
            [FromForm] string? response_url,
            [FromForm] string? trigger_id,
            [FromForm] string? user_id,
            [FromForm] string? user_name,
            [FromForm] string? team_id,
            [FromForm] string? team_domain,
            [FromForm] string? enterprise_id,
            [FromForm] string? enterprise_name,
            [FromForm] string? channel_id,
            [FromForm] string? channel_name,
            [FromForm] string? api_app_id)
        {

            var requestBody = await (new StreamReader(Request.Body)).ReadToEndAsync();
            await _queue.EnqueueAsync((token) => { return _slackService.HandleSlashCommandAsync(text, response_url, channel_id, user_id, trigger_id, token); });
            return Ok();
        }


        [HttpPost("actions")]
        [SlackAuthorize]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> IncomingAction([FromForm] string? payload)
        {
            string baseUri = $"{Request.Scheme}://{Request.Host}";

            await _queue.EnqueueAsync((token) => { return _slackService.HandleActionAsync(baseUri, payload, token); });
            return Ok();
        }
    }
}
