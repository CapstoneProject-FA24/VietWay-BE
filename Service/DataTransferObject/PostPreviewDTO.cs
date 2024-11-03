using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.Management.DataTransferObject
{
    public class PostPreviewDTO
    {
        public required string PostId { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? Content { get; set; }
        public string? PostCategory { get; set; }
        public string? Province { get; set; }
        public string? Description { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required PostStatus Status { get; set; }
    }
}
