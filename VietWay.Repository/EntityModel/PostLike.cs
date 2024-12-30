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
    [PrimaryKey(nameof(PostId), nameof(CustomerId))]
    public class PostLike
    {
        [StringLength(20)]
        [ForeignKey(nameof(Post))]
        [Required]
        public string? PostId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(Customer))]
        [Required]
        public string? CustomerId { get; set; }

        public required DateTime CreatedAt { get; set; }
        public virtual Post? Post { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
