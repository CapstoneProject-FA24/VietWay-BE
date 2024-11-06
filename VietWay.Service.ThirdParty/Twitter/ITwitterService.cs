using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;

namespace VietWay.Service.ThirdParty.Twitter
{
    public interface ITwitterService
    {
        public Task<string> PostTweetAsync(PostTweetRequestDTO postTweetRequestDTO);
    }
}
