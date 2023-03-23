using ImgBot.Api.Configuration;
using ImgBot.Api.Controllers;
using ImgBot.Api.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<BackgroundQueueService>();
builder.Services.AddHttpClient<SlackService>();
builder.Services.AddHttpClient<OpenAIService>();
builder.Services.AddHttpClient<AuthenticationController>();
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

builder.Services.AddCosmosCache(options => {
    var imgbotOptions = builder.Configuration.GetSection(ImgBotOptions.ImgBot).Get<ImgBotOptions>();

    if(imgbotOptions == null)
    {
        throw new Exception("Unable to get required options for ImgBot setup.");
    }

    options.ClientBuilder = new Microsoft.Azure.Cosmos.Fluent.CosmosClientBuilder(imgbotOptions.AzureCosmosDbConnectionString);
    options.DatabaseName = imgbotOptions.AzureCosmosDbName;
    options.ContainerName = imgbotOptions.AzureCosmosDbContainerName;
    options.CreateIfNotExists = true;
});

// Setup Serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

var app = builder.Build();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.ToString();
 
    await next();
});

// Configure the HTTP request pipeline.
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();
app.Run();
