﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Repository.EntityModel
{
    public class Province : SoftDeleteEntity
    {
        [Key]
        [StringLength(20)]
        public required string ProvinceId { get; set; }
        [StringLength(50)]
        public required string ProvinceName { get; set; }
        public required DateTime CreatedAt { get; set; }
        [StringLength(2048)]
        public required string ImageUrl { get; set; }
    }
}
