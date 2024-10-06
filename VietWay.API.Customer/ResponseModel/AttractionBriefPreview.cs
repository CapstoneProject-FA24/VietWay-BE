using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;

namespace VietWay.API.Customer.ResponseModel
{
    public class AttractionBriefPreview
    {
        public required string AttractionId { get; set; }
        public required string Name { get; set; }
    }
}
