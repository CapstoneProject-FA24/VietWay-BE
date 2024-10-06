using VietWay.Repository.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Interface
{
    public interface ITourService
    {
        public Task<Tour> CreateTour(Tour tour);
        public Task<Tour> EditTour(Tour updatedTour);
        public Task<(int totalCount, List<Tour> items)> GetAllTour(int pageSize, int pageIndex);
        public Task<Tour?> GetTourById(string id);
        public Task<(int totalCount, List<Tour> items)> GetAllScheduledTour(int pageSize, int pageIndex);
        public Task<(int totalCount, List<Tour> items)> GetAllToursByTemplateIdsAsync(
            string tourTemplateIds,
            int pageSize,
            int pageIndex);
    }
}
