using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Staff : CreatedByEntity<Manager>
    {
        [Key, ForeignKey(nameof(Account))]
        public long StaffId { get; set; }
        [Required]
        public required string FullName { get; set; }

        public virtual Account? Account { get; set; }
    }
}