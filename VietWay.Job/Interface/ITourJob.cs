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
        public Task OpenToursAsync();
        public Task CloseToursAsync();
        public Task ChangeToursToOngoingAsync();
        public Task ChangeToursToCompletedAsync();
        public Task RejectUnapprovedToursAsync();


    }
}
