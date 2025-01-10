using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.ThirdParty.Facebook;

namespace VietWay.Service.Management.DataTransferObject
{
    public class FacebookMetricsDTO
    {
        public int ImpressionCount { get; set; } = 0;
        public int ShareCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public PostReaction PostReactions { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string FacebookPostId { get; set; }
        public List<string> Hashtags { get; set; }
    }
}
