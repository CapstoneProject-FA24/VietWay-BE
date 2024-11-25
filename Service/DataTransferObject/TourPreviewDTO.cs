using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Management.DataTransferObject
{
    public class TourPreviewDTO
    {
        public string TourId { get; set; }
        public string TourTemplateId { get; set; }
        public string Code { get; set; }
        public string TourName { get; set; }
        public string Duration { get; set; }
        public string ImageUrl { get; set; }
        public string StartLocation { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? DefaultTouristPrice { get; set; }
        public int? MaxParticipant { get; set; }
        public int? MinParticipant { get; set; }
        public int? CurrentParticipant { get; set; }
        public TourStatus Status { get; set; }
    }
}
