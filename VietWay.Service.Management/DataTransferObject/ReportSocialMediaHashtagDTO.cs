using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class ReportSocialMediaHashtagDTO
    {
        public string? HashtagId { get; set; }
        public string? HashtagName { get; set; }
        public int? TotalXPost { get; set; }
        public int? TotalFacebookPost { get; set; }
        public decimal? AverageScore { get; set; }
        public decimal? AverageFacebookScore { get; set; }
        public decimal? AverageXScore { get; set; }
        public decimal? FacebookCTR { get; set; }
        public decimal? XCTR { get; set; }

    }
}
