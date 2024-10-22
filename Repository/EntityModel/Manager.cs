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
        public required string ManagerId { get; set; }
        [StringLength(100)]
        public required string FullName { get; set; }

        public virtual Account? Account { get; set; }
    }
}
