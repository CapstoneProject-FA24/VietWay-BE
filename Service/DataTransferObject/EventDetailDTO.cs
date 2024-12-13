using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.Management.DataTransferObject
{
    public class EventDetailDTO
    {
        public required string EventId { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? Content { get; set; }
        public string? EventCategoryId { get; set; }
        public string? EventCategory { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ProvinceId { get; set; }
        public string? ProvinceName { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
    }
}
