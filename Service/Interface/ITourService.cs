using VietWay.Repository.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Interface
{
    public interface ITourService
    {
        public Task<Tour> CreateTour(Tour tour);
        public Task<Tour> EditTour(Tour updatedTour);
        public Task<List<Tour>> GetAllTour(int pageSize, int pageIndex);
        public Task<Tour?> GetTourById(int id);
    }
}
