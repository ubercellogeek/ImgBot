using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImgBot.Api.Models.Slack
{
    public class Enterprise
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;
    }
}