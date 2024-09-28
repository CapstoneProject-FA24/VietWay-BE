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
    public class TourTemplateService : ITourTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TourTemplateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TourTemplate> CreateTourTemplate(TourTemplate template)
        {
            await _unitOfWork.TourTemplateRepository
                .Create(template);
            return template;
        }

        public async Task<TourTemplate> EditTourTemplate(TourTemplate updatedTemplate)
        {
            await _unitOfWork.TourTemplateRepository
                .Update(updatedTemplate);
            return updatedTemplate;
        }

        public async Task<List<TourTemplate>> GetAllTourTemplate(int pageSize, int pageIndex)
        {
            return await _unitOfWork.TourTemplateRepository
                .Query()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourCategory)
                .ToListAsync();
        }

        public async Task<TourTemplate?> GetTourTemplateById(int id)
        {
            return await _unitOfWork.TourTemplateRepository
                .Query()
                .Where(x => x.TourTemplateId.Equals(id))
                .Include(x => x.TourCategory)
                .FirstOrDefaultAsync();
        }
    }
}
