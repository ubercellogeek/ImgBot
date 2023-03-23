using ImgBot.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace ImgBot.Api.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IDistributedCache _cache;
    private readonly SlackOptions _options;

    public string? Nonce;
    public SlackOptions Options{ get { return _options; } }

    public IndexModel(ILogger<IndexModel> logger, IDistributedCache cache, IOptions<SlackOptions> options)
    {
        _cache = cache;
        _logger = logger;
        _options = options.Value;

    }

    public void OnGet()
    {

    }
}