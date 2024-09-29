using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.ModelEntity
{
    public class CustomerInfo
    {
        [Key, ForeignKey(nameof(Account))]
        public long CustomerId { get; set; }
        public required string FullName { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        [Required, ForeignKey(nameof(Province))]
        public long ProvinceId { get; set; }
        public Gender Gender { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Province? Province { get; set; }
    }
}
