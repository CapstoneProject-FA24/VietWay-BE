using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class ReportSocialMediaSummaryDTO
    {
        public List<string>? Dates { get; set; }
        public List<int>? FacebookComments { get; set; }
        public List<int>? FacebookShares { get; set; }
        public List<int>? FacebookReactions { get; set; }
        public List<int>? FacebookImpressions { get; set; }
        public List<decimal>? FacebookScore { get; set; }
        public List<int>? XRetweets { get; set; }
        public List<int>? XReplies { get; set; }
        public List<int>? XLikes { get; set; }
        public List<int>? XImpressions { get; set; }
        public List<decimal>? XScore { get; set; }
    }
}
