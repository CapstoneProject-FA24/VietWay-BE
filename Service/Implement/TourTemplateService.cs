using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class TourTemplateService(IUnitOfWork unitOfWork) : ITourTemplateService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task CreateTemplateAsync(TourTemplate template)
        {
            await _unitOfWork.TourTemplateRepository.Create(template);
        }
        public async Task UpdateTemplateAsync(TourTemplate template)
        {
            await _unitOfWork.TourTemplateRepository.Update(template);
        }
        public async Task SoftDeleteTemplateAsync(TourTemplate template)
        {
            await _unitOfWork.TourTemplateRepository.SoftDelete(template);
        }
        public async Task<(int totalCount, List<TourTemplate> items)> GetAllTemplatesAsync(int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .TourTemplateRepository
                .Query();
            int count = await query.CountAsync();
            List<TourTemplate> items = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourTemplateImages)
                .ThenInclude(x => x.Image)
                .Include(x => x.TourTemplateProvinces)
                .ThenInclude(x => x.Province)
                .Include(x => x.Creator)
                .ToListAsync();
            return (count, items);
        }

        public async Task<TourTemplate?> GetTemplateByIdAsync(long id)
        {
            return await _unitOfWork
                .TourTemplateRepository
                .Query()
                .Include(x => x.TourTemplateSchedules)
                .ThenInclude(x => x.AttractionSchedules)
                .Include(x => x.TourTemplateImages)
                .ThenInclude(x => x.Image)
                .Include(x => x.TourTemplateProvinces)
                .ThenInclude(x => x.Province)
                .Include(x=>x.Creator)
                .SingleOrDefaultAsync(x => x.TourTemplateId == id);
        }
    }
}
