using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class CustomerFeedbackService(IUnitOfWork unitOfWork): ICustomerFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<(int totalCount, List<CustomerFeedback> items)> GetAllCustomerFeedback(int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .CustomerFeedbackRepository
                .Query();
            int count = await query.CountAsync();
            List<CustomerFeedback> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Booking)
                .ToListAsync();
            return (count, items);
        }
    }
}
