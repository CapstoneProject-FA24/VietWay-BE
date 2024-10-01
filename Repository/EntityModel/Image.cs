using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    public class Image
    {
        [Key]
        [StringLength(20)]
        public required string ImageId { get; set; }
        [StringLength(64)]
        public required string SHA256 { get; set; }
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
