using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class AttractionScheduleService(IUnitOfWork unitOfWork): IAttractionScheduleService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<(int totalCount, List<AttractionSchedule> items)> GetAllAttractionSchedules(int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .AttractionScheduleRepository
                .Query();
            int count = await query.CountAsync();
            List<AttractionSchedule> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Attraction)
                .Include(x => x.TourTemplateSchedule)
                .ThenInclude(x => x.TourTemplate)
                .ToListAsync();
            return (count, items);
        }
    }
}
