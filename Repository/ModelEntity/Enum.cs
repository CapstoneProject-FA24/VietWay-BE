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
        public enum TourStatus
        {
            Scheduled,
            Closed,
            OnGoing,
            Completed,
            Cancelled,

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
        public enum BookingStatus
        {
            Pending,
            Confirmed,
            Cancelled,
            Completed,
            NoShow
        }
        public enum PaymentStatus
        {
            Pending,
            Paid,
            Cancelled
        }
    }
}
