using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Facebook
{
    public class FacebookApiConfig
    {
        public required string PageId { get; set; }
        public required string PageAccessToken { get; set; }
    }
}
