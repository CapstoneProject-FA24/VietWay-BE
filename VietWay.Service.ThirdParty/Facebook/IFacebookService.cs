using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Facebook
{
    public interface IFacebookService
    {
        public Task<string> PublishPostAsync(string content, string? url);
        public Task<int> GetPublishedPostReactionAsync(string facebookPostId);
    }
}
