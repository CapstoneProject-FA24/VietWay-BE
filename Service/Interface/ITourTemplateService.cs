using Repository.ModelEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ITourTemplateService
    {
        public Task<TourTemplate> CreateTourTemplate(TourTemplate template);
        public Task<TourTemplate> EditTourTemplate(TourTemplate updatedTemplate);
        public Task<List<TourTemplate>> GetAllTourTemplate(int pageSize, int pageIndex);
        public Task<TourTemplate?> GetTourTemplateById(int id);
    }
}
