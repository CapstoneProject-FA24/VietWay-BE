﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class PostReport
    {
        [Key]
        public string? ReportId { get; set; }
        [Required]
        public string? ReportLabel { get; set; }
        [Required]
        public ReportPeriod ReportPeriod { get; set; }
        [Required]
        [ForeignKey(nameof(Province))]
        [StringLength(20)]
        public string? ProvinceId { get; set; }
        [StringLength(20)]
        [ForeignKey(nameof(PostCategory))]
        [Required]
        public string? PostCategoryId { get; set; }
        public DateTime CreatedAt { get; set; }

        public int SiteReferralCount { get; set; }
        public int FacebookReferralCount { get; set; }
        public int XReferralCount { get; set; }

        public int SiteLikeCount { get; set; }

        public int FacebookClickCount { get; set; }
        public int FacebookImpressionCount { get; set; }
        public int FacebookLikeCount { get; set; }
        public int FacebookLoveCount { get; set; }
        public int FacebookWowCount { get; set; }
        public int FacebookHahaCount { get; set; }
        public int FacebookSorryCount { get; set; }
        public int FacebookAngerCount { get; set; }
        public int FacebookShareCount { get; set; }
        public int FacebookCommentCount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int FacebookReactionCount { get; set; }

        public int XRetweetCount { get; set; }
        public int XReplyCount { get; set; }
        public int XLikeCount { get; set; }
        public int XQuoteCount { get; set; }
        public int XBookmarkCount { get; set; }
        public int XImpressionCount { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double FacebookScore { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double XScore { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double SiteScore { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double AverageScore { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double FacebookCTR { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double XCTR { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]

        public virtual Province? Province { get; set; }
        public virtual PostCategory? PostCategory { get; set; }
    }
}
