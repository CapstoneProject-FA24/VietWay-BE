using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IEventCategoryService
    {
        public Task<List<EventCategoryPreviewDTO>> GetAllEventCategoryPreviewAsync();
    }
}
