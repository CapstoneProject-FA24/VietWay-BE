using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Facebook
{
    public class FacebookApiConfig(string pageId, string pageAccessToken)
    {
        private readonly string _pageId = pageId;
        private readonly string _pageAccessToken = pageAccessToken;
        public string PageId { get => _pageId; }
        public string PageAccessToken { get => _pageAccessToken; }
    }
}
