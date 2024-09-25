using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class TourGuideInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TourGuideId { get; set; }
        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public int CompanyId { get; set; }
        public required string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime AddedDate { get; set; }
        [ForeignKey(nameof(CompanyStaffInfo))]
        public int AddedBy { get; set; }

        public virtual CompanyStaffInfo? CompanyStaffInfo { get; set; }
        public virtual Account? Account { get; set; }
        public virtual Company? Company { get; set; }
    }
}
