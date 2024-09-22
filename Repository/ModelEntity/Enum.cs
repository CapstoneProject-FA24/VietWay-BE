using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelEntity
{
    public class Enum
    {
        public enum AccountStatus
        {
            Inactive,
            Active
        }
        public enum Gender
        {
            Male,
            Female,
            Other
        }
        public enum UserRole
        {
            Customer,
            TourGuide,
            CompanyStaff,
            Manager,
            Admin
        }
        public enum TourTemplateStatus
        {
            Draft,
            Pending,
            Approved,
            Rejected
        }
        public enum CompanyStatus
        {
            Inactive,
            Active
        }
        public enum AttractionStatus
        {
            Inactive,
            Active
        }
    }
}
