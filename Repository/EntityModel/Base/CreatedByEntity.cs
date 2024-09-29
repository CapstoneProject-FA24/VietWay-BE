using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.EntityModel.Base
{
    public class CreatedByEntity<T> : SoftDeleteEntity where T : class
    {
        [ForeignKey(nameof(Creator))]
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual T? Creator { get; }
    }
}