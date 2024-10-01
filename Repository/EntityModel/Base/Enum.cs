using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel.Base
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }
    public enum UserRole
    {
        Customer,
        Staff,
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
    public enum AttractionStatus
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
