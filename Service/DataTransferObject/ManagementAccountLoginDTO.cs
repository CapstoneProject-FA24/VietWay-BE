using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.DataTransferObject
{
    public class ManagementAccountLoginDTO
    {
        public string Token { get; set; }
        public UserRole Role { get; set; }
    }
}
