using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class ManagerInfo
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManagerId { get; set; }
        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public required string FullName { get; set; }

        public virtual Account? Account { get; set; }
    }
}
