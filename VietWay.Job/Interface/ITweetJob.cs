using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;

namespace VietWay.Job.Interface
{
    public interface ITweetJob
    {
        [AutomaticRetry(Attempts = 0)]
        public Task GetPublishedTweetsJob();
        [AutomaticRetry(Attempts = 0)]
        public Task GetPopularHashtagJob();
    }
}
