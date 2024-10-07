using CloudinaryDotNet;

namespace VietWay.API.Management.ResponseModel
{
    public class ManagerInfoPreview
    {
        public required string ManagerId { get; set; }
        public required string FullName { get; set; }
        public Account? Account { get; set; }
    }
}
