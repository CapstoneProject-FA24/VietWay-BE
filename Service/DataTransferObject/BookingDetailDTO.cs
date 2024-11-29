using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Management.DataTransferObject
{
    public class BookingDetailDTO
    {
        public string BookingId { get; set; }
        public string? TourId { get; set; }
        public string? TourName { get; set; }
        public string? TourCode { get; set; }
        public DateTime? StartDate { get; set; }
        public string? StartLocation { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ContactFullName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public string? ContactAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal PaidAmount { get; set; }
        public int NumberOfParticipants { get; set; }
        public BookingStatus Status { get; set; }
        public string? Note { get; set; }
        public List<BookingTouristDetailDTO> Tourists { get; set; }
        public List<BookingPaymentDetailDTO> Payments { get; set; }
        public decimal? RefundAmount { get; set; }
        public DateTime? CancelAt { get; set; }
        public UserRole? CancelBy { get; set; }
        public List<TourPolicyPreview>? TourPolicies { get; set; }
    }

    public class BookingTouristDetailDTO
    {
        public string TouristId { get; set; }
        public string? FullName { get; set; }
        public Gender Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal Price { get; set; }
        public string PIN { get; set; }
    }

    public class BookingPaymentDetailDTO
    {
        public string PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }
        public DateTime CreateAt { get; set; }
        public string? BankCode { get; set; }
        public string? BankTransactionNumber { get; set; }
        public DateTime? PayTime { get; set; }
        public string? ThirdPartyTransactionNumber { get; set; }
        public PaymentStatus Status { get; set; }
    }

    public class TourPolicyPreview
    {
        public DateTime? CancelBefore { get; set; }
        public decimal? RefundPercent { get; set; }
    }
}