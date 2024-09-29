using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.ModelEntity
{
    public class Account : SoftDeleteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AccountId { get; set; }
        [Required]
        [MaxLength(10)]
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
        public required DateTime CreatedAt { get; set; }
        public UserRole Role { get; set; }
    }
}
