using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class ReportRevenueDTO
    {
        public required decimal TotalRevenue { get; set; }
        public required ReportRevenueByPeriod ReportRevenueByPeriod { get; set; }
        public required List<ReportRevenueByTourTemplate> ReportRevenueByTourTemplate { get; set; }
        public required List<ReportRevenueByTourCategory> ReportRevenueByTourCategory { get; set; }
    }
    public class ReportRevenueByTourTemplate
    {
        public required string TourTemplateName { get; set; }
        public required decimal TotalRevenue { get; set; }
    }
    public class ReportRevenueByTourCategory
    {
        public required string TourCategoryName { get; set; }
        public required decimal TotalRevenue { get; set; }
    }
    public class ReportRevenueByPeriod
    {
        public required List<string> Periods { get; set; }
        public required List<decimal> Revenue { get; set; }
        public required List<decimal> Refund { get; set; }
    }
}
