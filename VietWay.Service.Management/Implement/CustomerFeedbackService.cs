﻿using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.Interface;

namespace VietWay.Service.Management.Implement
{
    public class CustomerFeedbackService(IUnitOfWork unitOfWork) : ICustomerFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<(int totalCount, List<TourReview> items)> GetAllCustomerFeedback(int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .TourReviewRepository
                .Query();
            int count = await query.CountAsync();
            List<TourReview> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Booking)
                .ToListAsync();
            return (count, items);
        }
    }
}
