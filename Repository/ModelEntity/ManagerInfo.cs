using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.ModelEntity
{
    public class ManagerInfo
    {
        [Key, ForeignKey(nameof(Account))]
        public long ManagerId { get; set; }
        public required string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey(nameof(Creator))]
        public long CreatedBy { get; set; }
        public virtual Account? Account { get; set; }
        public virtual AdminInfo? Creator { get; set; }
    }
}
