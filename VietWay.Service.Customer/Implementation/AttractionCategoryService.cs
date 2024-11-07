using Microsoft.EntityFrameworkCore;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.Service.Customer.Implementation
{
    public class AttractionCategoryService(IUnitOfWork unitOfWork) : IAttractionCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<AttractionCategoryPreviewDTO>> GetAllAttractionCategoriesAsync()
        {
            return await _unitOfWork.AttractionCategoryRepository
                .Query()
                .Select(x => new AttractionCategoryPreviewDTO
                {
                    AttractionCategoryId = x.AttractionCategoryId,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToListAsync();
        }
    }
}
