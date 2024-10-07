using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface ITourDurationService
    {
        public Task<List<TourDuration>> GetAllTourDuration();
    }
}
