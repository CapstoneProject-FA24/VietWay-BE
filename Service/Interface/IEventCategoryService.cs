using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.DataTransferObject;

namespace VietWay.Service.Interface
{
    public interface IEventCategoryService
    {
        public Task<List<EventCategoryPreviewDTO>> GetAllEventCategoryPreviewAsync();
    }
}
