using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class CompanyStaffInfo
    {
        [Key,ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public required string FullName { get; set; }
        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Company? Company { get; set; }
    }
}
