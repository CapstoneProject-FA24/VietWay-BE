using Microsoft.EntityFrameworkCore;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.Service.Customer.Implementation
{
    public class EventCategoryService(IUnitOfWork unitOfWork) : IEventCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<EventCategoryPreviewDTO>> GetEventCategoriesAsync()
        {
            return await _unitOfWork.EventCategoryRepository.Query()
                .Where(x => false == x.IsDeleted)
                .Select(x => new EventCategoryPreviewDTO
                {
                    EventCategoryId = x.EventCategoryId,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();
        }
    }
}
