using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class PostPreviewDTO
    {
        public required string PostId { get; set; }
        public required string Title { get; set; }
        public required string ImageUrl { get; set; }
        public required string PostCategoryName { get; set; }
        public required string ProvinceName { get; set; }
        public required string Description { get; set; }
        public bool IsLiked { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}
