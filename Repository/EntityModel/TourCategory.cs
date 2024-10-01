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
    public class TourCategory : CreatedByEntity<Manager>
    {
        [Key]
        [StringLength(20)]
        public required string TourCategoryId { get; set; }
        [StringLength(255)]
        public required string Name { get; set; }
    }
}
