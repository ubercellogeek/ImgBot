using ImgBot.Api.Configuration;
using ImgBot.Api.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<BackgroundQueueService>();
builder.Services.AddHttpClient<SlackService>();
builder.Services.AddHttpClient<OpenAIService>();
builder.Services.AddSingleton<ISlackService, SlackService>();
builder.Services.AddSingleton<IOpenAIService, OpenAIService>();
builder.Services.AddMemoryCache();

// Setup configuration
builder.Services.Configure<OpenAIOptions>(
    builder.Configuration.GetSection(OpenAIOptions.OpenAI));

builder.Services.Configure<SlackOptions>(
    builder.Configuration.GetSection(SlackOptions.Slack));

builder.Services.Configure<ImgBotOptions>(
    builder.Configuration.GetSection(ImgBotOptions.ImgBot));

// Setup Serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
