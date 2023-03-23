namespace ImgBot.Api.Configuration
{
    public class ImgBotOptions
    {
        public const string ImgBot = "ImgBot";
        public int WorkerCount { get; set; } = 10;
        public string? AzureBlobStorageConnectionString { get; set; }
        public string? AzureCosmosDbConnectionString { get; set; }
        public string? AzureCosmosDbName { get; set; }
        public string? AzureCosmosDbContainerName { get; set; }
    }
}