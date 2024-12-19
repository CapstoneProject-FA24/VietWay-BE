using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class ReportBookingDTO
    {
        public int TotalBooking { get; set; }
        public required ReportBookingByDay ReportBookingByDay { get; set; }
        public required List<ReportBookingByTourTemplate> ReportBookingByTourTemplate { get; set; }
        public required List<ReportBookingByTourCategory> ReportBookingByTourCategory { get; set; }
        public required List<ReportBookingParticipantCountDTO> ReportBookingByParticipantCount { get; set; }
    }
    public class ReportBookingByTourTemplate
    {
        public required string TourTemplateName { get; set; }
        public required int TotalBooking { get; set; }
    }
    public class ReportBookingByTourCategory
    {
        public required string TourCategoryName { get; set; }
        public required int TotalBooking { get; set; }
    }
    public class ReportBookingParticipantCountDTO
    {
        public int ParticipantCount { get; set; }
        public int BookingCount { get; set; }
    }
    public class ReportBookingByParticipantAge
    {
        public required string ParticipantAge { get; set; }
        public required int TotalBooking { get; set; }
    }
    public class ReportBookingByDay
    {
        public required List<string> Dates { get; set; }
        public required List<int> PendingBookings { get; set; }
        public required List<int> DepositedBookings { get; set; }
        public required List<int> PaidBookings { get; set; }
        public required List<int> CompletedBookings { get; set; }
        public required List<int> CancelledBookings { get; set; }
    }
}
