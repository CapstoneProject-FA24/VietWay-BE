using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel;

namespace VietWay.API.Management.RequestModel
{
    public class CreateProvinceRequest
    {
        public required string ProvinceId { get; set; }
        public required string Name { get; set; }
    }
}
