using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class ReportSocialMediaProvinceDetailDTO : ReportSocialMediaProvinceDTO
    {
        public ReportSocialMediaSummaryDTO? ReportSocialMediaSummary { get; set; }
        public List<ReportSocialMediaAttractionCategoryDTO>? AttractionCategories { get; set; }
        public List<ReportSocialMediaTourCategoryDTO>? TourTemplateCategories { get; set; }
        public List<ReportSocialMediaPostCategoryDTO>? PostCategories { get; set; }

    }
}
