using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class BookingDetailDTO
    {
        public required string BookingId { get; set; }
        public required string TourId { get; set; }
        public required string CustomerId { get; set; }
        public required int NumberOfParticipants { get; set; }
        public required string ContactFullName { get; set; }
        public required string ContactEmail { get; set; }
        public required string ContactPhoneNumber { get; set; }
        public string? ContactAddress { get; set; }
        public required decimal TotalPrice { get; set; }
        public required BookingStatus Status { get; set; }
        public required DateTime CreatedOn { get; set; }
        public string? Note { get; set; }

        public required string StartLocation { get; set; }
        public required DateTime StartDate { get; set; }

        public required string TourName { get; set; }
        public required string ImageUrl { get; set; }
        public required string Code { get; set; }

        public required ICollection<TourParticipantDTO> Participants { get; set; }
    }
}
