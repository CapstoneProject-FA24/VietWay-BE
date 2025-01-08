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
    [PrimaryKey(nameof(TourTemplateId), nameof(HashtagId))]
    public class TourTemplateHashtag
    {
        [ForeignKey(nameof(TourTemplate))]
        [Required]
        public string TourTemplateId { get; set; }
        [ForeignKey(nameof(Hashtag))]
        [Required]
        public string HashtagId { get; set; }
    }
}
