using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface IAttractionScheduleService
    {
        public Task<(int totalCount, List<AttractionSchedule> items)> GetAllAttractionSchedules(int pageSize, int pageIndex);
    }
}
