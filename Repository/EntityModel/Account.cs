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
    public class Account : SoftDeleteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AccountId { get; set; }
        [Required]
        [RegularExpression(@"^0\d{9}$")]
        public required string PhoneNumber { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public required string Password { get; set; }
        public required DateTime CreatedAt { get; set; }
        public UserRole Role { get; set; }
    }
}
