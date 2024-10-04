using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Management.ResponseModel
{
    public class AttractionTypePreview
    {
        public required string AttractionTypeId { get; set; }
        public required string AttractionTypeName { get; set; }
    }
}
