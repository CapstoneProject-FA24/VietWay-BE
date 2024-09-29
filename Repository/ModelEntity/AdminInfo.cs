using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.ModelEntity
{
    public class AdminInfo
    {
        [Key, ForeignKey(nameof(Account))]
        public long AdminId { get; set; }
        public required string FullName { get; set; }

        public virtual Account? Account { get; set; }
    }
}
