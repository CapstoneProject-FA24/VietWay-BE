using Microsoft.EntityFrameworkCore;
using Repository.ModelEntity;
using Repository.UnitOfWork;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implement
{
    public class TourService : ITourService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TourService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Tour> CreateTour(Tour tour)
        {
            await _unitOfWork.TourRepository
                .Create(tour);
            return tour;
        }

        public async Task<Tour> EditTour(Tour updatedTour)
        {
            await _unitOfWork.TourRepository
                .Update(updatedTour);
            return updatedTour;
        }

        public async Task<List<Tour>> GetAllTour(int pageSize, int pageIndex)
        {
            return await _unitOfWork.TourRepository
                .Query()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourTemplate)
                .ToListAsync();
        }

        public async Task<Tour?> GetTourById(int id)
        {
            return await _unitOfWork.TourRepository
                .Query()
                .Where(x => x.TourId.Equals(id))
                .Include(x => x.TourTemplate)
                .FirstOrDefaultAsync();
        }
    }
}
