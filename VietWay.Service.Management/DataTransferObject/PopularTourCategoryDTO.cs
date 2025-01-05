using System.Text.Json.Serialization;

namespace VietWay.Service.Management.DataTransferObject
{
    public class PopularTourCategoryDTO
    {
        [JsonPropertyName("tourCategoryId")]
        public string TourCategoryId { get; set; }

        [JsonConstructor]
        public PopularTourCategoryDTO()
        {
            TourCategoryId = string.Empty;
        }

        public PopularTourCategoryDTO(string tourCategoryId)
        {
            TourCategoryId = tourCategoryId;
        }
    }
}
