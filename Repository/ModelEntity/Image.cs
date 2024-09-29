using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.ModelEntity
{
    public class Image
    {
        [Key]
        public long ImageId { get; set; }
        public required string PublicId { get; set; }
        public required string Url { get; set; }
        public required string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
