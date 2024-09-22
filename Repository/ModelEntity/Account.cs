using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class Account
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
        public required Enum.AccountStatus AccountStatus { get; set; }
        public Enum.UserRole Role { get; set; }
    }
}
