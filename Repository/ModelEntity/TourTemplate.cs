using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class TourTemplate
    {
        public int TourTemplateId { get; set; }
        public string TourName { get; set; }
        public string Description { get; set; }
        public string TourTemplateDuration { get; set; }
        public string TourTemplateCategory { get; set; }
        public string TourTemplateFacility { get; set; }
        public string TourTemplatePolicy { get; set; }
        public string TourTemplateNote { get; set; }
        public string TourTemplateStatus { get; set; }
        public string TourTemplateCreatedDate { get; set; }
        public string TourTemplateUpdatedDate { get; set; }
        public string TourTemplateCreatedBy { get; set; }
        public string TourTemplateUpdatedBy { get; set; }
        
    }
}
