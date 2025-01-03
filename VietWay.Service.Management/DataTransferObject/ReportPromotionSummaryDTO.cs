using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class ReportPromotionSummaryDTO
    {
        public int TotalFacebookPost { get; set; }
        public int FacebookImpressionCount { get; set; }
        public int FacebookReferralCount { get; set; }
        public int FacebookCommentCount { get; set; }
        public int FacebookShareCount { get; set; }
        public int FacebookReactionCount { get; set; }
        public int TotalXPost { get; set; }
        public int XRetweetCount { get; set; }
        public int XReplyCount { get; set; }
        public int XLikeCount { get; set; }
        public int XImpressionCount { get; set; }
        public int XReferralCount { get; set; }
    }
}
        