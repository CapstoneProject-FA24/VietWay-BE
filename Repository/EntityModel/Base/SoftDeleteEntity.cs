using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel.Base
{
    public class SoftDeleteEntity
    {
        [Required]
        public bool IsDeleted { get; set; }
    }
}
