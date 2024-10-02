using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
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
        public async Task<(int totalCount, List<TourTemplate> items)> GetAllTemplatesAsync(
            string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<string>? durationIds,
            TourTemplateStatus? status,
            int pageSize,
            int pageIndex)
        {
            var query = _unitOfWork
                .TourTemplateRepository
                .Query()
                .Where(x=>x.IsDeleted == false);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.TourName.Contains(nameSearch));
            }
            if (templateCategoryIds != null && templateCategoryIds.Count > 0)
            {
                query = query.Where(x => templateCategoryIds.Contains(x.TourCategoryId));
            }
            if (provinceIds != null && provinceIds.Count > 0)
            {
                query = query.Where(x=>x.TourTemplateProvinces.Select(x=>x.ProvinceId).Any(p=>provinceIds.Contains(p)));
            }
            if (durationIds != null && durationIds.Count > 0)
            {
                query = query.Where(x => durationIds.Contains(x.DurationId));
            }
            if (status != null)
            {
                query = query.Where(x => x.Status.Equals(status));
            }
            int count = await query.CountAsync();
            List<TourTemplate> items = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourTemplateImages)
                .ThenInclude(x => x.Image)
                .Include(x => x.TourTemplateProvinces)
                .ThenInclude(x => x.Province)
                .Include(x=>x.TourCategory)
                .Include(x=>x.TourDuration)
                .Include(x => x.Creator)
                .ToListAsync();
            return (count, items);
        }

        public async Task<TourTemplate?> GetTemplateByIdAsync(string id)
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
                .Include(x=>x.TourDuration)
                .Include(x=>x.TourCategory)
                .Include(x=>x.Creator)
                .SingleOrDefaultAsync(x => x.TourTemplateId.Equals(id));
        }
    }
}
