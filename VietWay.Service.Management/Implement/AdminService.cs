using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.Interface;
using VietWay.Util.CustomExceptions;

namespace VietWay.Service.Management.Implement
{
    public class AdminService(IUnitOfWork unitOfWork) : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<int> GetCustomerAnalytic(DateTime? startDate, DateTime? endDate)
        {
            var query = _unitOfWork
                .CustomerRepository
                .Query();

            if (startDate == null)
            {
                throw new InvalidInfoException(nameof(startDate));
            }
            else if (endDate == null)
            {
                throw new InvalidInfoException(nameof(endDate));
            }

            int total = await query
                .Include(x => x.Account)
                .Where(x => x.Account.CreatedAt > startDate && x.Account.CreatedAt < endDate)
                .CountAsync();
            return total;
        }
    }
}
