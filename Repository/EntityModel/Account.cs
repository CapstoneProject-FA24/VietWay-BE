using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Account : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        public required string AccountId { get; set; }
        [StringLength(10)]
        public required string PhoneNumber { get; set; }
        [StringLength(320)]
        public string? Email { get; set; }
        [StringLength(60)]
        public required string Password { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required UserRole Role { get; set; }
    }
}
