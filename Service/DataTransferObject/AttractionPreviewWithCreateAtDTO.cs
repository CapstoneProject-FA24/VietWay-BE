using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.DataTransferObject
{
    public class AttractionPreviewWithCreateAtDTO
    {
        public required string AttractionId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Province { get; set; }
        public string? AttractionType { get; set; }
        public AttractionStatus Status { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
