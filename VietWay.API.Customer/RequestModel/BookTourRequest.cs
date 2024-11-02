using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Customer.RequestModel
{
    public class BookTourRequest
    {
        public required string TourId { get; set; }
        public required int NumberOfParticipants { get; set; }
        [StringLength(100)]
        public required string ContactFullName { get; set; }
        [StringLength(320)]
        public required string ContactEmail { get; set; }
        [StringLength(10)]
        public required string ContactPhoneNumber { get; set; }
        [StringLength(255)]
        public string? ContactAddress { get; set; }
        public string? Note { get; set; }
        public List<TourParticipant>? TourParticipants { get; set; }
    }
    public class TourParticipant
    {
        public required string FullName { get; set; }
        [StringLength(10)]
        public required string PhoneNumber { get; set; }
        public required Gender Gender { get; set; }
        public required DateTime DateOfBirth { get; set; }
    }
}
