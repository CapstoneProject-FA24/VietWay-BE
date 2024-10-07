using System.ComponentModel.DataAnnotations;

namespace VietWay.Repository.EntityModel.Base
{
    public class SoftDeleteEntity
    {
        [Required]
        public bool IsDeleted { get; set; }
    }
}
