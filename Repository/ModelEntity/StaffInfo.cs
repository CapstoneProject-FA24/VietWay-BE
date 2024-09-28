using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.ModelEntity
{
    public class StaffInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StaffId { get; set; }
        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public required string FullName { get; set; }

        public virtual Account? Account { get; set; }
    }
}