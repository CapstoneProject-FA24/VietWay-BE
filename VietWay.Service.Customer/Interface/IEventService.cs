using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IEventService
    {
        Task<EventDetailDTO> GetEventDetailAsync(string eventId);
        Task<List<EventPreviewDTO>> GetEventsAsync(string? nameSearch, List<string>? provinceIds, List<string>? eventCategoryIds, DateTime? startDateFrom, DateTime? startDateTo, int checkedPageSize, int checkedPageIndex);
    }
}
