using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class ReportRatingDTO
    {
        public required List<AttractionRatingDTO> AttractionRatingInPeriod { get; set; }
        public required List<TourTemplateRatingDTO> TourTemplateRatingInPeriod { get; set; }
        public required List<AttractionRatingDTO> AttractionRatingTotal { get; set; }
        public required List<TourTemplateRatingDTO> TourTemplateRatingTotal { get; set; }
    }
    public class AttractionRatingDTO
    {
        public string? AttractionName { get; set; }
        public double? AverageRating { get; set; }
        public int? TotalRating { get; set; }
    }
    public class TourTemplateRatingDTO
    {
        public string? TourTemplateName { get; set; }
        public double? AverageRating { get; set; }
        public int? TotalRating { get; set; }
    }
}
