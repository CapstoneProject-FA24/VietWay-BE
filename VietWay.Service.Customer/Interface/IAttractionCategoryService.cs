using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IAttractionCategoryService
    {
        public Task<List<AttractionCategoryPreviewDTO>> GetAllAttractionType();
    }
}
