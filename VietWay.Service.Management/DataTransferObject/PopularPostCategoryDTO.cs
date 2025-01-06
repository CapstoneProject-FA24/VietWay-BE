using System.Text.Json.Serialization;

namespace VietWay.Service.Management.DataTransferObject
{
    public class PopularPostCategoryDTO
    {
        [JsonPropertyName("postCategoryId")]
        public string PostCategoryId { get; set; }

        [JsonConstructor]
        public PopularPostCategoryDTO()
        {
            PostCategoryId = string.Empty;
        }

        public PopularPostCategoryDTO(string postCategoryId)
        {
            PostCategoryId = postCategoryId;
        }
    }
}
