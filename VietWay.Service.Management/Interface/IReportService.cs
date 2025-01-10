using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IReportService
    {
        public Task<ReportSummaryDTO> GetReportSummaryAsync(DateTime startDate, DateTime endDate);
        public Task<ReportBookingDTO> GetReportBookingAsync(DateTime startDate, DateTime endDate);
        public Task<ReportRatingDTO> GetReportRatingAsync(DateTime startDate, DateTime endDate, bool isAsc);
        public Task<ReportRevenueDTO> GetReportRevenueAsync(DateTime startDate, DateTime endDate);
        public Task<ReportPromotionSummaryDTO> GetPromotionSummaryAsync(DateTime startDate, DateTime endDate);
        public Task<ReportSocialMediaSummaryDTO> GetSocialMediaSummaryAsync(DateTime startDate, DateTime endDate);
        public Task<List<ReportSocialMediaProvinceDTO>> GetSocialMediaProvinceReport(DateTime startDate, DateTime endDate);
        public Task<List<ReportSocialMediaPostCategoryDTO>> GetSocialMediaPostCategoryReport(DateTime startDate, DateTime endDate);
        public Task<List<ReportSocialMediaAttractionCategoryDTO>> GetSocialMediaAttractionCategoryReport(DateTime startDate, DateTime endDate);
        public Task<List<ReportSocialMediaTourCategoryDTO>> GetSocialMediaTourTemplateCategoryReport(DateTime startDate, DateTime endDate);
        public Task<List<ReportSocialMediaHashtagDTO>> GetSocialMediaHashtagReport(DateTime startDate, DateTime endDate);
        public Task<ReportSocialMediaProvinceDetailDTO> GetSocialMediaProvinceDetailReport(DateTime startDate, DateTime endDate, string provinceId);
        public Task<ReportSocialMediaAttractionCategoryDetailDTO> GetSocialMediaAttractionCategoryDetailReport(DateTime startDate, DateTime endDate, string attractionCategoryId);
        public Task<ReportSocialMediaPostCategoryDetailDTO> GetSocialMediaPostCategoryDetailReport(DateTime startDate, DateTime endDate, string postCategoryId);
        public Task<ReportSocialMediaTourCategoryDetailDTO> GetSocialMediaTourCategoryDetailReport(DateTime startDate, DateTime endDate, string tourCategoryId);
    }
}
