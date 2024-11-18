using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Job.DataTransferObject
{
    public class TweetDTO
    {
        public string PostId { get; set; }
        public string XTweetId { get; set; }
        public int RetweetCount { get; set; }
        public int ReplyCount { get; set; }
        public int LikeCount { get; set; }
        public int QuoteCount { get; set; }
        public int BookmarkCount { get; set; }
        public int ImpressionCount { get; set; }
    }
}
