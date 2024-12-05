using VietWay.Repository.EntityModel;

namespace VietWay.Service.Management.Interface
{
    public interface ICustomerFeedbackService
    {
        public Task<(int totalCount, List<TourReview> items)> GetAllCustomerFeedback(int pageSize, int pageIndex);
    }
}
