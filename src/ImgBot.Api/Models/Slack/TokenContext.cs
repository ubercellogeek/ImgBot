using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImgBot.Api.Models.Slack
{
    public class TokenContext
    {
        public string UserId { get; set; } = default!;
        public string TeamId { get; set; } = default!;
        public string? EnterpriseId { get; set; }
        public string UserToken { get; set; } = default!;
        
    }
}