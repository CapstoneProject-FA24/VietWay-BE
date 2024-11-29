using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Management.RequestModel
{
    public class ZaloPayCallbackData
    {
        [JsonProperty("data")]
        public string Data { get; set; }
        [JsonProperty("mac")]
        public string Mac { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
    }
}
