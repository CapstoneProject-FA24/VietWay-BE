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
    public class BookingPreviewDTO
    {
        public string BookingId { get; set; }
        public string? TourId { get; set; }
        public string? TourName { get; set; }
        public string? TourCode { get; set; }
        public string? Duration { get; set; }
        public List<string>? Provinces { get; set; }
        public DateTime? StartDate { get; set; }
        public string? StartLocation { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ContactFullName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal PaidAmount { get; set; }
        public int NumberOfParticipants { get; set; }
        public BookingStatus Status { get; set; }
        public required bool HavePendingRefund { get; set; }
        public List<BookingTouristPreviewDTO> Tourists { get; set; }
    }

    public class BookingTouristPreviewDTO
    {
        public string TouristId { get; set; }
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PIN { get; set; }
    }
}
