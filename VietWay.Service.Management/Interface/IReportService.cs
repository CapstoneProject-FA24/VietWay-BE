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
        Task<ReportPromotionSummaryDTO> GetPromotionSummaryAsync(DateTime startDate, DateTime endDate);
    }
}
