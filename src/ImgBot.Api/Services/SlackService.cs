using System.Text.Json;
using ImgBot.Api.Models.OpenAI;
using ImgBot.Api.Models.Slack;
using ImgBot.Api.Models.ImgBot;
using ImgBot.Api.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using ImgBot.Api.Extensions;

namespace ImgBot.Api.Services
{
    public class SlackService : ISlackService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IBackgroundTaskQueue _queue;
        private readonly IOpenAIService _openAI;
        private readonly IDistributedCache _cache;
        private readonly SlackOptions _slackOptions;
        private readonly ImgBotOptions _imgBotOptions;


        public SlackService(IHttpClientFactory clientFactory, IBackgroundTaskQueue queue, IOpenAIService openAI, IDistributedCache cache, IOptions<SlackOptions> slackOptions, IOptions<ImgBotOptions> imgBotOptions)
        {
            _clientFactory = clientFactory;
            _queue = queue;
            _openAI = openAI;
            _cache = cache;
            _slackOptions = slackOptions.Value;
            _imgBotOptions = imgBotOptions.Value;
        }

        public async Task HandleSlashCommandAsync(string? text, string? respondUri, string? channelId, string? userId, string? teamId, string? enterpriseId, string? triggerId, CancellationToken cancellationToken)
        {
            if(
                string.IsNullOrWhiteSpace(text) || 
                string.IsNullOrWhiteSpace(respondUri) ||
                string.IsNullOrWhiteSpace(userId)) 
            {
                return;
            }

            var context = await _cache.GetJsonAsync<TokenContext>($"TOKEN_{userId}{teamId}{enterpriseId}", cancellationToken);
            
            if(context == null)
            {
                // Request user_scope=chat:write

                var authReturnGuid = Guid.NewGuid();
                var key = $"USER_AUTH_REQUEST_{authReturnGuid}";

                await _cache.SetStringAsync(key, respondUri, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });
                //_cache.Set(key, respondUri, new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)});

                var message = new Message();
                message.Blocks = new object[] {
                new Section() {
                    Text = new PlainText("Before you can start posting the images you generate, ImgBot requires your permission to do so on your behalf. If you belong to multiple workpaces, remember to select this workpspace to complete the authorization process.")
                },
                new ActionsBlock() {
                    Elements = new object[] {
                        new Button() { Style = "primary", Text = new PlainText("Authorize"), Url = $"{SlackOptions.OAuth2AuthorizeUri}?client_id={_slackOptions.ClientId}&user_scope=chat:write&redirect_uri={_slackOptions.OAuth2RedirectUri}&state={key}", ActionId = "ok_authorize", Value = $"USER_AUTH_REQUEST_{authReturnGuid}"},
                        new Button() { Style = "danger", Text = new PlainText("Cancel"), ActionId = "cancel_auth", Value = "cancel_auth" }
                    }
                }};

                await SendPayload(respondUri, message, cancellationToken);
                return;
            }

            await GenerateImagesAndPostAsync(text, respondUri, cancellationToken: cancellationToken);
        }

        public async Task HandleActionAsync(string? baseUri, string? payload, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(payload) || string.IsNullOrWhiteSpace(baseUri))
            {
                return;
            }

            var action = JsonSerializer.Deserialize<ActionResponse>(payload);

            // Fast return for invalid state
            if (action == null || string.IsNullOrWhiteSpace(action?.ResponseUrl))
            {
                return;
            }

            var actionId = action.Actions?.FirstOrDefault()?.ActionId;
            var value = action.Actions?.FirstOrDefault()?.Value;

            if (actionId == "cancel" && !string.IsNullOrWhiteSpace(value))
            {
                await DeleteMessageAsync(action.ResponseUrl, cancellationToken);
                await _cache.RemoveAsync(value);
                return;
            }

            if (actionId == "post" && !string.IsNullOrWhiteSpace(action.Channel?.Id))
            {
                var previewId = value?.Split('.').FirstOrDefault();
                var imagePreviewId = value?.Split('.').LastOrDefault();

                var preview = await _cache.GetJsonAsync<PayloadPreview>(previewId ?? string.Empty);

                if (preview != null &&
                    !string.IsNullOrWhiteSpace(imagePreviewId) && preview != null)
                {
                    var ctx = await GetTokenContextAsync(action.User.Id, action.Team.Id, action.Enterprise?.Id);

                    if(ctx != null)
                    {
                        await PostChannelMessageAsync(action.User, ctx, baseUri, preview, imagePreviewId, action.Channel.Id, cancellationToken);
                        await DeleteMessageAsync(action.ResponseUrl, cancellationToken);
                    }  
                }
            }

            if (actionId == "regenerate" && !string.IsNullOrWhiteSpace(value))
            {
                var preview = await _cache.GetJsonAsync<PayloadPreview>(value);
                if (preview != null)
                {
                    await DeleteMessageAsync(action.ResponseUrl, cancellationToken);
                    await GenerateImagesAndPostAsync(preview.Prompt, preview.ReplyToUri, preview.Id, cancellationToken);
                }
            }

            if(actionId =="ok_authorize" && !string.IsNullOrWhiteSpace(value))
            {
                await _cache.SetStringAsync(value, action.ResponseUrl);
            }

            if(actionId == "ok_error")
            {
                await DeleteMessageAsync(action.ResponseUrl, cancellationToken);
            }

            if(actionId == "cancel_auth")
            {
                await DeleteMessageAsync(action.ResponseUrl, cancellationToken);
            }

            if(actionId == "ok_auth_complete")
            {
                await DeleteMessageAsync(action.ResponseUrl, cancellationToken);
            }
        }

        public async Task SendPayload<T>(string uri, T payload, CancellationToken cancellationToken)
        {
            using var httpClient = _clientFactory.CreateClient();
            var result = await httpClient.PostAsJsonAsync(uri, payload, new System.Text.Json.JsonSerializerOptions(), cancellationToken);
            var rs = await result.Content.ReadAsStringAsync();
        }

        public async Task SendMessageAsync(User user, TokenContext context, Message message, CancellationToken cancellationToken)
        {
            using var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.UserToken);
            var result = await httpClient.PostAsJsonAsync(SlackOptions.PostMessageUri, message, cancellationToken);
            var rss = await result.Content.ReadAsStringAsync();
            Console.WriteLine(rss);
        }

        public async Task DeleteMessageAsync(string replyUri, CancellationToken cancellationToken)
        {
            await SendPayload(replyUri, new Message() { DeleteMessage = true }, cancellationToken);
        }

        private async Task GenerateImagesAndPostAsync(string text, string respondUri, string? previewId = default, CancellationToken cancellationToken = default)
        {
            // Fast return if no response uri was provided.
            if (string.IsNullOrWhiteSpace(respondUri))
            {
                return;
            }

            // Fast return if we don't have anything to operate on.
            if (string.IsNullOrWhiteSpace(text))
            {
                await SendErrorAsync(respondUri, "Received blank prompt text. You must provide prompt text to generate an image.", cancellationToken);
                return;
            }

            var numImages = 1;
            var openAIResult = new ImageGenerationResult();

            try
            {
                openAIResult = await _openAI.GenerateImageAsync(new ImageGenerationRequest() { Size = "512x512", Prompt = text, Number = numImages }, cancellationToken);
            }
            catch (System.Exception)
            {
                // Send back message to slack that something went wrong.
                await SendErrorAsync(respondUri, $"There was an error while generating the image.", cancellationToken);
                return;
            }


            if (openAIResult.StatusCode >= 400)
            {
                // Send back message to slack that something went wrong.
                await SendErrorAsync(respondUri, $"Recieved error from Open AI request. Status code: {openAIResult.StatusCode}. Message: {openAIResult.RawData}", cancellationToken);
                return;
            }
            
            PayloadPreview? preview = default;

            if(!string.IsNullOrWhiteSpace(previewId))
            {
                preview = await _cache.GetJsonAsync<PayloadPreview>(previewId);
            }

            if(preview != null)
            {
                preview.RegenerateCount += 1;
                preview.Images.Clear();
                preview.Images = openAIResult.Items.Select(x => new ImagePreview() { Url = x.Url }).ToList();
            }
            else
            {
                preview = new PayloadPreview() { Prompt = text, ReplyToUri = respondUri, Images = openAIResult.Items.Select(x => new ImagePreview() { Url = x.Url }).ToList() };
            }

            await SendPreviewAsync(preview, cancellationToken);
            await _cache.SetJsonAsync(preview.Id, preview, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)});
        }

        private async Task SendErrorAsync(string uri, string errorMessage, CancellationToken cancellationToken)
        {
            var message = new Message();
            message.Blocks = new object[] {
                new Section() {
                    Text = new PlainText(errorMessage)
                },
                new ActionsBlock() {
                    Elements = new object[] {
                        new Button() { Style = "primary", Text = new PlainText("Ok"), ActionId = "ok_error", Value = "ok_error"}
                    }
                }
            };

            await SendPayload(uri, message, cancellationToken);
        }

        private async Task SendPreviewAsync(PayloadPreview preview, CancellationToken cancellationToken)
        {
            var message = new Message();
            var blocks = new List<object>();

            blocks.Add(
                new ContextBlock()
                {
                    Elements = new object[] {
                            new MarkdownText() { Text = preview.Prompt }
                    }
                }
            );

            preview.Images.ForEach(x =>
            {
                var elements = new List<object>();
                blocks.Add(new Image() { Url = x.Url });

                elements.Add(
                    new Button()
                    {
                        Text = new PlainText()
                        {
                            Text = "Post",
                            Emoji = true,
                        },
                        Style = "primary",
                        Value = $"{preview.Id}.{x.Id}",
                        ActionId = "post"
                    });
                
                if(preview.RegenerateCount < 4)
                {
                    elements.Add(
                        new Button()
                        {
                            Text = new PlainText()
                            {
                                Text = "Regenerate",
                                Emoji = true,
                            },
                            Style = "primary",
                            Value = preview.Id,
                            ActionId = "regenerate"
                        });
                }

                elements.Add(
                    new Button()
                    {
                        Text = new PlainText()
                        {
                            Text = "Cancel",
                            Emoji = true,
                        },
                        Style = "danger",
                        Value = preview.Id,
                        ActionId = "cancel"
                    });

                blocks.Add(new ActionsBlock()
                {
                    Elements = elements.ToArray()
                });
            });

            message.Blocks = blocks.ToArray();

            await SendPayload(preview.ReplyToUri, message, cancellationToken);

            await _cache.SetJsonAsync<PayloadPreview>(preview.Id, preview, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)});
        }

        private async Task PostChannelMessageAsync(User user, TokenContext context, string baseUri, PayloadPreview preview, string imagePreviewId, string channelId, CancellationToken cancellationToken)
        {
            var imageName = $"{Guid.NewGuid()}.png";
            var blobClient = new Azure.Storage.Blobs.BlobClient(_imgBotOptions.AzureBlobStorageConnectionString, "images", imageName);
            var dlImageUrl = preview.Images.Where(x=>x.Id == imagePreviewId).Select(y=>y.Url).First();

            if(string.IsNullOrWhiteSpace(dlImageUrl))
            {
                return;
            }

            var info = blobClient.SyncCopyFromUri(new Uri(dlImageUrl));

            var channelMessage = new Message()
            {
                Blocks = new object[] {
                        new ContextBlock() { Elements = new object[] {
                            new MarkdownText { Text = preview!.Prompt }
                        }},
                        new Image() { Url = blobClient.Uri.ToString() },
                        new ContextBlock() { Elements = new object[] {
                            new Image() { Url = $"{baseUri}/imgbot.png", AltText = "imgbot" },
                            new PlainText("Generated by ImgBot")
                        }}
                    }
            };

            channelMessage.ResponseType = "in_channel";
            channelMessage.Channel = channelId;

            await SendMessageAsync(user, context, channelMessage, cancellationToken);
        }

        public async Task<TokenContext?> GetTokenContextAsync(string userId, string teamId, string? enterpriseId = null)
        {
            var cacheKey = $"TOKEN_{userId}{teamId}{enterpriseId}";

            var context = await _cache.GetJsonAsync<TokenContext>(cacheKey);

            return context;
        }

        public async Task SetTokenContextAsync(TokenContext context)
        {
            var cacheKey = $"TOKEN_{context.UserId}{context.TeamId}{context.EnterpriseId}";

            await _cache.SetJsonAsync(cacheKey, context);
        }
    }
}