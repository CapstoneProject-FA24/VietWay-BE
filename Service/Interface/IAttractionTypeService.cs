using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IAttractionTypeService
    {
        public Task<List<AttractionCategoryPreviewDTO>> GetAllAttractionType();
    }
}
