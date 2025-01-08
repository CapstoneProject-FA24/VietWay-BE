using System.Text.Json.Serialization;

namespace VietWay.Service.Management.DataTransferObject
{
    public class PopularProvinceDTO
    {
        [JsonPropertyName("provinceId")]
        public string ProvinceId { get; set; }

        [JsonConstructor]
        public PopularProvinceDTO()
        {
            ProvinceId = string.Empty;
        }

        public PopularProvinceDTO(string provinceId)
        {
            ProvinceId = provinceId;
        }
    }
}
