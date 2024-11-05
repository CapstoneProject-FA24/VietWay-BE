using Microsoft.EntityFrameworkCore;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.Service.Management.Implement
{
    public class EventCategoryService(IUnitOfWork unitOfWork) : IEventCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<EventCategoryDTO>> GetAllEventCategoryAsync()
        {
            return await _unitOfWork.EventCategoryRepository.Query()
                .Select(x => new EventCategoryDTO
                {
                    EventCategoryId = x.EventCategoryId,
                    Name = x.Name,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                }).ToListAsync();
        }
    }
}
