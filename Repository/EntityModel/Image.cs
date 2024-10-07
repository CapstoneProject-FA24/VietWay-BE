using System.ComponentModel.DataAnnotations;

namespace VietWay.Repository.EntityModel
{
    public class Image
    {
        [Key]
        [StringLength(20)]
        public required string ImageId { get; set; }
        [StringLength(255)]
        public required string PublicId { get; set; }
        [StringLength(255)]
        public required string FileName { get; set; }
        [StringLength(2048)]
        public required string Url { get; set; }
        [StringLength(100)]
        public required string ContentType { get; set; }
    }
}
