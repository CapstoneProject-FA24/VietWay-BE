using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Customer : SoftDeleteEntity
    {
        [Key, ForeignKey(nameof(Account))]
        public long CustomerId { get; set; }
        [Required]
        public required string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        [Required, ForeignKey(nameof(Province))]
        public long ProvinceId { get; set; }
        public Gender Gender { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Province? Province { get; set; }
    }
}
