using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Facebook
{
    public class PostReaction
    {
        [JsonPropertyName("like")]
        public int LikeCount { get; set; } = 0;
        [JsonPropertyName("love")]
        public int LoveCount { get; set; } = 0;
        [JsonPropertyName("wow")]
        public int WowCount { get; set; } = 0;
        [JsonPropertyName("haha")]
        public int HahaCount { get; set; } = 0;
        [JsonPropertyName("sad")]
        public int SadCount { get; set; } = 0;
        [JsonPropertyName("anger")]
        public int AngryCount { get; set; } = 0; 

    }
}
