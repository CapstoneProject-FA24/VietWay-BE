using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;

namespace VietWay.Job.Interface
{
    public interface ITourJob
    {
        public Task OpenTourAsync(string tourId);
        public Task CloseTourAsync(string tourId);
        public Task ChangeTourToOngoingAsync(string tourId);
        public Task ChangeTourToCompletedAsync(string tourId);
        
    }
}
