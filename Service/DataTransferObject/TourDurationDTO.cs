namespace VietWay.Service.Management.DataTransferObject
{
    public class TourDurationDTO
    {
        public required string DurationId { get; set; }
        public string? DurationName { get; set; }
        public int NumberOfDay { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
