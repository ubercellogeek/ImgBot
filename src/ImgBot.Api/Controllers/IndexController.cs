using ImgBot.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImgBot.Api.Controllers
{
    public class IndexController : ControllerBase
    {
        private readonly IBackgroundTaskQueue _queue;
        public IndexController(IBackgroundTaskQueue queue)
        {
            _queue = queue;
        }

        [HttpGet("")]
        public IActionResult Index()
        { 
            return Ok(nameof(ImgBot));
        }
    }
}