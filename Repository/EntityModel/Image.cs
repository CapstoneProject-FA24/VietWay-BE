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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ImageId { get; set; }
        [Required]
        public required string SHA256 { get; set; }
        [Required]
        public required string PublicId { get; set; }
        [Required]
        public required string Url { get; set; }
        [Required]
        public required string ContentType { get; set; }
    }
}
