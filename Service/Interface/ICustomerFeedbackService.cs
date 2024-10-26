﻿using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface ICustomerFeedbackService
    {
        public Task<(int totalCount, List<TourReview> items)> GetAllCustomerFeedback(int pageSize, int pageIndex);
    }
}
