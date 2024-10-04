using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface ITourCategoryService
    {
        public Task<List<TourCategory>> GetAllTourCategory();
    }
}
