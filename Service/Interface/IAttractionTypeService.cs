using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface IAttractionTypeService
    {
        public Task<List<AttractionType>> GetAllAttractionType();
    }
}
