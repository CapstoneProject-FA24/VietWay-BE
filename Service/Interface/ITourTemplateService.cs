using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.ModelEntity;

namespace VietWay.Service.Interface
{
    public interface ITourTemplateService
    {
        public Task<(int totalCount, List<TourTemplate> items)> GetAllTemplatesAsync(int pageSize, int pageIndex);
        public Task<TourTemplate?> GetTemplateByIdAsync(long id);
    }
}
