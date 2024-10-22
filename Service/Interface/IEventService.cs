using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.DataTransferObject;

namespace VietWay.Service.Interface
{
    public interface IEventService
    {
        public Task<List<EventPreviewDTO>> GetAllActiveEvent(string? nameSearch, List<string>? provinceIds,
            List<string>? eventCategoryIds, DateTime? startDateFrom, DateTime? startDateTo,
            int pageSize, int pageIndex);
    }
}
