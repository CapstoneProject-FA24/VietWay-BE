using Microsoft.EntityFrameworkCore;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class TourCategoryService(IUnitOfWork unitOfWork) : ITourCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<Repository.EntityModel.TourCategory>> GetAllTourCategory()
        {
            return await _unitOfWork.TourCategoryRepository
                .Query()
                .ToListAsync();
        }
    }
}
