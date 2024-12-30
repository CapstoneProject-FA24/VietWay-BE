﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel
{
    public class PostMetric
    {
        [Key]
        [StringLength(20)]
        public string? MetricId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(Post))]
        public string? PostId { get; set; }
        public int? NewViewCount { get; set; }
        public int? NewSaveCount { get; set; }
        public int? FacebookReferralCount { get; set; }
        public int? XReferralCount { get; set; }
        public int? Score { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Post? Post { get; set; }
    }
}
