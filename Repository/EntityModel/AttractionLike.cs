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
    [PrimaryKey(nameof(AttractionId), nameof(CustomerId))]
    public class AttractionLike
    {
        [ForeignKey(nameof(Attraction))]
        [Required]
        public string? AttractionId { get; set; }
        [ForeignKey(nameof(Customer))]
        [Required]
        public string? CustomerId { get; set; }

        public virtual Attraction? Attraction { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
