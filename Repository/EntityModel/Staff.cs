using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Staff : SoftDeleteEntity
    {
        [Key]
        [ForeignKey(nameof(Account))]
        [StringLength(20)]
        [Required]
        public string? StaffId { get; set; }
        [StringLength(100)]
        [Required]
        public string? FullName { get; set; }

        public virtual Account? Account { get; set; }
    }
}