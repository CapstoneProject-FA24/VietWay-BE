using VietWay.Repository.EntityModel;
using VietWay.Service.DataTransferObject;

namespace VietWay.Service.Interface
{
    public interface IAttractionTypeService
    {
        public Task<List<AttractionCategoryPreviewDTO>> GetAllAttractionType();
    }
}
