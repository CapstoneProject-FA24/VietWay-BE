namespace VietWay.Service.Management.DataTransferObject
{
    public class ReportSummaryDTO
    {
        public int NewCustomer { get; set; }
        public int NewBooking { get; set; }
        public int NewTour { get; set; }
        public decimal Revenue { get; set; }
        public int NewAttraction { get; set; }
        public int NewPost { get; set; }
        public decimal AverateTourRating { get; set; }
    }
}
