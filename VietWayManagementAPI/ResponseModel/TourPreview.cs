using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;

namespace VietWay.API.Management.ResponseModel
{
    public class TourPreview
    {
        public required string TourId { get; set; }
        public required string TourTemplateId { get; set; }
        public required string StartLocation { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
        public required decimal Price { get; set; }
        public required int MaxParticipant { get; set; }
        public required int MinParticipant { get; set; }
        public required int CurrentParticipant { get; set; }
        public required TourStatus Status { get; set; }
    }
}
