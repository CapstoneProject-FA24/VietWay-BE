using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.Interface
{
    public interface IAdminService
    {
        public Task<int> GetCustomerAnalytic(DateTime? startDate, DateTime? endDate);
    }
}
