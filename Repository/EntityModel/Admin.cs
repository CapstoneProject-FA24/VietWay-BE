using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.EntityModel
{
    public class Admin
    {
        [Key]
        [ForeignKey(nameof(Account))]
        [StringLength(20)]
        public required string AdminId { get; set; }
        [StringLength(100)]
        public required string FullName { get; set; }

        public virtual Account? Account { get; set; }
    }
}
