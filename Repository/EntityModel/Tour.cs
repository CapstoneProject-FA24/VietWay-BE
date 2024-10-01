﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Tour : CreatedByEntity<Staff>
    {
        [Key]
        [StringLength(20)]
        public required string TourId { get; set; }
        [ForeignKey(nameof(TourTemplate))]
        [StringLength(20)]
        public required string TourTemplateId { get; set; }
        [StringLength(255)]
        public required string StartLocation { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Price { get; set; }
        public required int MaxParticipant { get; set; }
        public required int MinParticipant { get; set; }
        public required int CurrentParticipant { get; set; }
        public required TourStatus Status { get; set; }

        public virtual TourTemplate? TourTemplate { get; set; }
        public virtual ICollection<TourBooking>? Bookings { get; set; }

    }
}
