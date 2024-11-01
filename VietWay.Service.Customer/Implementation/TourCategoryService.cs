using Microsoft.EntityFrameworkCore;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.Service.Customer.Implementation
{
    public class TourCategoryService(IUnitOfWork unitOfWork) : ITourCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<TourCategoryDTO>> GetTourCategories()
        {
            return await _unitOfWork.TourCategoryRepository.Query()
                .Where(x => false == x.IsDeleted)
                .Select(x => new TourCategoryDTO
                {
                    TourCategoryId = x.TourCategoryId,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();
        }
    }
}
