using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;
using VietWay.Util.DateTimeUtil;

namespace VietWay.Service.Implement
{
    internal class EventService(IUnitOfWork unitOfWork, ITimeZoneHelper timeZoneHelper) : IEventService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task<List<EventPreviewDTO>> GetAllActiveEvent(string? nameSearch, List<string>? provinceIds, 
            List<string>? eventCategoryIds, DateTime? startDateFrom, DateTime? startDateTo, int pageSize, int pageIndex)
        {
            IQueryable<Event> query = _unitOfWork.EventRepository.Query()
                .Where(x => x.Status == EventStatus.Approved && x.IsDeleted == false);
            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(x => x.Title.Contains(nameSearch));
            }
            if (provinceIds?.Count >0)
            {
                query = query.Where(x => provinceIds.Contains(x.ProvinceId));
            }
            if (eventCategoryIds?.Count > 0)
            {
                query = query.Where(x => eventCategoryIds.Contains(x.EventCategoryId));
            }
            if (startDateFrom != null)
            {
                query = query.Where(x => x.StartDate >= startDateFrom);
            } 
            else
            {
                query = query.Where(x => x.StartDate >= _timeZoneHelper.GetUTC7Now());
            }
            if (startDateTo != null)
            {
                query = query.Where(x => x.StartDate <= startDateTo);
            }
            int count = await query.CountAsync();
            List<EventPreviewDTO> list = await query
                .OrderBy(x=>x.StartDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new EventPreviewDTO()
                {
                    EventId = x.EventId,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    EventCategory = x.EventCategory.Name,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ProvinceName = x.Province.ProvinceName,
                    Description = x.Description
                }).ToListAsync();
            return list;
        }
    }
}
