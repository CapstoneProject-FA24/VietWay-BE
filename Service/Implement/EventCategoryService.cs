using Microsoft.EntityFrameworkCore;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class EventCategoryService(IUnitOfWork unitOfWork) : IEventCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<EventCategoryPreviewDTO>> GetAllEventCategoryPreviewAsync()
        {
            return await _unitOfWork.EventCategoryRepository.Query()
                .Select(x => new EventCategoryPreviewDTO
                {
                    EventCategoryId = x.EventCategoryId,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();
        }
    }
}
