using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Interface
{
    public interface ITourTemplateService
    {
        public Task<(int totalCount, List<TourTemplate> items)> GetAllTemplatesAsync(
            string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<string>? durationIds,
            TourTemplateStatus? status,
            int pageSize,
            int pageIndex);
        public Task<TourTemplate?> GetTemplateByIdAsync(string id);
        public Task CreateTemplateAsync(TourTemplate tourTemplate); 
    }
}
