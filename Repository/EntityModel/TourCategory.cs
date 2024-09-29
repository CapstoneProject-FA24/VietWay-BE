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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TourCategoryId { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}
