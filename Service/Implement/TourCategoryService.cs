using Microsoft.EntityFrameworkCore;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.Service.Management.Implement
{
    public class TourCategoryService(IUnitOfWork unitOfWork) : ITourCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

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
