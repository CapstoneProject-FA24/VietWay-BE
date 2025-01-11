using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Twitter
{
    public class TweetDTO
    {
        public string? XTweetId { get; set; }
        public int RetweetCount { get; set; }
        public int ReplyCount { get; set; }
        public int LikeCount { get; set; }
        public int QuoteCount { get; set; }
        public int BookmarkCount { get; set; }
        public int ImpressionCount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<string> Hashtags { get; set; }
    }

    public class HashtagCountDTO
    {
        public string HashtagId { get; set; }
        public int Count { get; set; }
        public bool IsCurrent { get; set; }
    }
}
