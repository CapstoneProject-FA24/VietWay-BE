using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class AttractionTypeService(IUnitOfWork unitOfWork): IAttractionTypeService
    {
        public readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<AttractionCategory>> GetAllAttractionType()
        {
            return await _unitOfWork.AttractionCategoryRepository
                .Query()
                .ToListAsync();
        }
    }
}
