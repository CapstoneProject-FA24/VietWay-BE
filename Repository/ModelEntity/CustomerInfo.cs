using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class CustomerInfo
    {
        [Key,ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public required string FullName { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        [Required, ForeignKey(nameof(Province))]
        public int ProvinceId { get; set; }
        public Enum.Gender Gender { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Province? Province { get; set; }
    }
}
