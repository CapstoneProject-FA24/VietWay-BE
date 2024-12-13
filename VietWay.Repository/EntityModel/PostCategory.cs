using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class PostCategory : SoftDeleteEntity
    {
        [Key]
        [Required]
        [StringLength(20)]
        public string? PostCategoryId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }
    }
}
