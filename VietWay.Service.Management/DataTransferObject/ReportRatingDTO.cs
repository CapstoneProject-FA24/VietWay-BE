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
        public required string AttractionName { get; set; }
        public required double AverageRating { get; set; }
        public required int TotalRating { get; set; }
    }
    public class TourTemplateRatingDTO
    {
        public required string TourTemplateName { get; set; }
        public required double AverageRating { get; set; }
        public required int TotalRating { get; set; }
    }
}
