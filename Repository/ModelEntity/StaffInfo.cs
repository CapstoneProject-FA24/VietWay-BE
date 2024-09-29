using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.Repository.ModelEntity
{
    public class StaffInfo
    {
        [Key, ForeignKey(nameof(Account))]
        public long StaffId { get; set; }
        public required string FullName { get; set; }

        public virtual Account? Account { get; set; }
    }
}