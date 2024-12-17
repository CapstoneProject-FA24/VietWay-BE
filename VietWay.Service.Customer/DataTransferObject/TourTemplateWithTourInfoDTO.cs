using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class TourTemplateWithTourInfoDTO
    {
        public required string TourTemplateId { get; set; }
        public required string Code { get; set; }
        public required string TourName { get; set; }
        public required string Duration { get; set; }
        public required string TourCategory { get; set; }
        public required string StartingProvince { get; set; }
        public required List<string> Provinces { get; set; }
        public required string ImageUrl { get; set; }
        public required decimal MinPrice { get; set; }
        public required List<DateTime> StartDate { get; set; }
        public double? AverageRating { get; set; }
        public string? Transportation { get; set; }
    }
}
