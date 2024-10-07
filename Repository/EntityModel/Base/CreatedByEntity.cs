using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.EntityModel.Base
{
    public class CreatedByEntity<T> : SoftDeleteEntity where T : class
    {
        [ForeignKey(nameof(Creator))]
        [Required]
        [StringLength(20)]
        public required string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        public virtual T? Creator { get; set; }
    }
}