using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface ICustomerFeedbackService
    {
        public Task<(int totalCount, List<CustomerFeedback> items)> GetAllCustomerFeedback(int pageSize, int pageIndex);
    }
}
