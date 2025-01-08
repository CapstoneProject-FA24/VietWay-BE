using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class HashtagReport
    {
        [Key]
        public string? ReportId { get; set; }
        [Required]
        public string? ReportLabel { get; set; }
        [Required]
        public ReportPeriod ReportPeriod { get; set; }
        [Required]
        [ForeignKey(nameof(Hashtag))]
        public string? HashtagId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public int FacebookReferralCount { get; set; }
        public int XReferralCount{ get; set; }

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
        public double FacebookCTR { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double XCTR { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]

        public virtual Hashtag? Hashtag { get; set; }
    }
}
