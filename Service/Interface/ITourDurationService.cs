using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface ITourDurationService
    {
        public Task<List<TourDurationDTO>> GetAllTourDuration();
    }
}
