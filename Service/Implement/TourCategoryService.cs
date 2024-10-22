using Microsoft.EntityFrameworkCore;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class TourCategoryService(IUnitOfWork unitOfWork) : ITourCategoryService
    {
        public readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<TourCategoryDTO>> GetAllTourCategory()
        {
            return await _unitOfWork.TourCategoryRepository
                .Query()
                .Select(x => new TourCategoryDTO()
                {
                    Description = x.Description,
                    Name = x.Name,
                    TourCategoryId = x.TourCategoryId
                }).ToListAsync();
        }
    }
}
