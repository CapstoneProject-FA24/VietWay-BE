using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Job.DataTransferObject;

namespace VietWay.Job.Interface
{
    public interface ITweetJob
    {
        public Task GetPublishedTweetsJob();
    }
}
