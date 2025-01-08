using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class ReportSocialMediaTourCategoryDTO
    {
        public string? TourCategoryId { get; set; }
        public string? TourCategoryName { get; set; }
        public int? TotalTourTemplate { get; set; }
        public int? TotalXPost { get; set; }
        public int? TotalFacebookPost { get; set; }
        public decimal? AverageScore { get; set; }
        public decimal? AverageFacebookScore { get; set; }
        public decimal? AverageXScore { get; set; }
        public decimal? AverageTourTemplateScore { get; set; }
    }
}
