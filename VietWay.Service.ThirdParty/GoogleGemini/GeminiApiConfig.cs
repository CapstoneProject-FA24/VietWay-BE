using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.GoogleGemini
{
    public class GeminiApiConfig
    {
        public required string ApiKey { get; set; }
        public string? SystemPrompt { get; set; }
    }
}
