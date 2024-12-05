using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Management.RequestModel
{
    public class CreateTourDurationRequest
    {
        public string? DurationName { get; set; }
        public int NumberOfDay { get; set; }
    }
}
