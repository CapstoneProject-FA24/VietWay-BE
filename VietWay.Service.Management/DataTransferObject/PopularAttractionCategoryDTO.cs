using System.Text.Json.Serialization;

namespace VietWay.Service.Management.DataTransferObject
{
    public class PopularAttractionCategoryDTO
    {
        [JsonPropertyName("attractionCategoryId")]
        public string AttractionCategoryId { get; set; }

        [JsonConstructor]
        public PopularAttractionCategoryDTO()
        {
            AttractionCategoryId = string.Empty;
        }

        public PopularAttractionCategoryDTO(string attractionCategoryId)
        {
            AttractionCategoryId = attractionCategoryId;
        }
    }
}
