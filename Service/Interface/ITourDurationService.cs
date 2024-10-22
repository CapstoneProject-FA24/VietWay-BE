using VietWay.Repository.EntityModel;
using VietWay.Service.DataTransferObject;

namespace VietWay.Service.Interface
{
    public interface ITourDurationService
    {
        public Task<List<TourDuration>> GetAllTourDuration();

        public Task<List<TourDurationPreviewDTO>> GetTourDurationPreviews();
    }
}
