using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class CustomerFeedbackService(IUnitOfWork unitOfWork): ICustomerFeedbackService
    {
        public readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<(int totalCount, List<Feedback> items)> GetAllCustomerFeedback(int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .FeedbackRepository
                .Query();
            int count = await query.CountAsync();
            List<Feedback> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Booking)
                .ToListAsync();
            return (count, items);
        }
    }
}
