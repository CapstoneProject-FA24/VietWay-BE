using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface IProvinceService
    {
        public Task<List<Province>> GetAllProvince();
        public Task<Province?> GetProvinceById(long id);
    }
}
