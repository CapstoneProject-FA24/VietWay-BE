using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class CredentialDTO
    {
        public required string Token { get; set; }
        public required string FullName { get; set; }
        public required string AvatarUrl { get; set; } = "https://upload.wikimedia.org/wikipedia/commons/2/2c/Default_pfp.svg";
    }
}
