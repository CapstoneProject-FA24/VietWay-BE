using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Management.DataTransferObject
{
    public class PostDetailDTO
    {
        public required string PostId { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? Content { get; set; }
        public string? PostCategoryId { get; set; }
        public string? ProvinceId { get; set; }
        public string? Description { get; set; }
        public string? ProvinceName { get; set; }
        public string? PostCategoryName { get; set; }
        public DateTime CreateAt { get; set; }
        public PostStatus Status { get; set; }
    }
}
