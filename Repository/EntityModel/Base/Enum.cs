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
        Accepted,
        Opened,
        Closed,
        OnGoing,
        Completed,
        Cancelled,
    }
    public enum BookingStatus
    {
        Pending,
        Deposited,
        Paid,
        Completed,
        Cancelled,
        PendingChangeConfirmation,
    }
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded
    }
    public enum EntityType
    {
        Attraction,
        AttractionType,
        Booking,
        Customer,
        Event,
        Manager,
        Post,
        Province,
        Staff,
        Tour,
        TourBooking,
        TourCategory,
        TourDuration,
        TourTemplate,
        BookingRefund
    }
    public enum EventStatus
    {
        Draft,
        Pending,
        Approved,
        Rejected
    }
    public enum PostStatus
    {
        Draft, //0
        Pending, //1 
        Approved, //2
        Rejected //3
    }
    public enum EntityModifyAction
    {
        Create,
        Update,
        ChangeStatus,
        Delete
    }
    public enum PaymentMethod
    {
        VNPay,
        Momo,
        ZaloPay,
        PayOS
    }
    public enum RefundStatus
    {
        Pending,
        Refunded,
    }
}
