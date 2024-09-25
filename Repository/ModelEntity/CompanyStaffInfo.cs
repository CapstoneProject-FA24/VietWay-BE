using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.ModelEntity
{
    public class CompanyStaffInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StaffId { get; set; }
        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public required string FullName { get; set; }
        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Company? Company { get; set; }
    }
}