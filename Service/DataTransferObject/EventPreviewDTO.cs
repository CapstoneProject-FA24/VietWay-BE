using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.DataTransferObject
{
    public class EventPreviewDTO
    {
        public required string EventId { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? EventCategory { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ProvinceName { get; set; }
        public string? Description { get; set; }
    }
}
