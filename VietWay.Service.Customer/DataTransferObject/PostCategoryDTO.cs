using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class PostCategoryDTO
    {
        public required string PostCategoryId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
