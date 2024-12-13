using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Manager : SoftDeleteEntity
    {
        [Key]
        [ForeignKey(nameof(Account))]
        [StringLength(20)]
        [Required]
        public string? ManagerId { get; set; }
        [StringLength(100)]
        [Required]
        public string? FullName { get; set; }

        public virtual Account? Account { get; set; }
    }
}
