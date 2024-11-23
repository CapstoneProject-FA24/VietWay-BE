using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Twitter
{
    public class PostTweetRequestDTO
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonIgnore]
        public string? ImageUrl { get; set; }
    }

}
