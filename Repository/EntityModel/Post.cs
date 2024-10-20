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
    public class Post : SoftDeleteEntity
    {
        [Key]
        public required string PostId { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? Content { get; set; }
        [ForeignKey(nameof(PostCategory))]
        public string? PostCategoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [ForeignKey(nameof(Province))]
        public string? ProvinceId { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public required DateTime CreatedAt { get; set; }

        public virtual PostCategory? PostCategory { get; set; }
        public virtual Province? Province { get; set; }
    }
}
