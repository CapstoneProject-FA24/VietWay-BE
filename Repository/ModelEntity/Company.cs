using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class Company
    {
        public int CompanyId { get; set; }
        public required string CompanyName { get; set; }
        public required string CompanyAddress { get; set; }
        public required string CompanyPhone { get; set; }
        public required string CompanyEmail { get; set; }
        public string? CompanyWebsite { get; set; }
        public required string CompanyLogo { get; set; }
        public required string CompanyDescription { get; set; }
        public required DateTime CreateDate { get; set; }

        public Enum.CompanyStatus CompanyStatus { get; set; }
    }
}
