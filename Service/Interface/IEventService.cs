using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IEventService
    {
        public Task<List<EventPreviewDTO>> GetAllActiveEvent(string? nameSearch, List<string>? provinceIds,
            List<string>? eventCategoryIds, DateTime? startDateFrom, DateTime? startDateTo,
            int pageSize, int pageIndex);

        public Task<EventDetailDTO?> GetEventDetailAsync(string eventId);
    }
}
