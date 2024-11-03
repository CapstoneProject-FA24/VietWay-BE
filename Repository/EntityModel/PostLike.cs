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
        public required string PostId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(Customer))]
        public required string CustomerId { get; set; }


        public virtual Post? Post { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
