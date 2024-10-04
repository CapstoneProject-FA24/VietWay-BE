using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Management.ResponseModel
{
    public class AttractionPreview
    {
        public required string AttractionId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string Province { get; set; }
        public required string AttractionType { get; set; }
        public required AttractionStatus Status { get; set; }
        public required string ImageUrl { get; set; }
    }
}
