using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    public class Hashtag
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? HashtagId { get; set; }
        [StringLength(30)]
        public string? HashtagName { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
