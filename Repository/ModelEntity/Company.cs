using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class Company
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string ContactInfo { get; set; }
        public string? Website { get; set; }
        public required string Logo { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedDate { get; set; }
        public Enum.CompanyStatus CompanyStatus { get; set; }
        
        public virtual ICollection<CompanyStaffInfo>? CompanyStaffs { get; set; }
        public virtual ICollection<TourTemplate>? TourTemplates { get; set; }
        public virtual ICollection<TourGuideInfo>? TourGuides { get; set; }
    }
}
