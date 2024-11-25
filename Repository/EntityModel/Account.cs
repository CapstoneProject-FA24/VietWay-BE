using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Account : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        [Required]
        public string? AccountId { get; set; }
        [StringLength(10)]
        [Required]
        public string? PhoneNumber { get; set; }
        [StringLength(320)]
        public string? Email { get; set; }
        [StringLength(60)]
        [Required]
        public string? Password { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public UserRole Role { get; set; }
    }
}
