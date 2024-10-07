﻿namespace VietWay.Repository.EntityModel.Base
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }
    public enum UserRole
    {
        Customer, //0
        Staff, //1
        Manager, //2
        Admin //3
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
        Pending,
        Rejected,
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
        Completed
    }
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed
    }
}
