using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface IManagerService
    {
        public Task<List<Manager>> GetAllManagerInfos(int pageSize, int pageIndex);
        public Task<Manager?> GetManagerInfoById(int id);
        public Task<Manager> EditManagerInfo(Manager managerInfo);
        public Task<Manager> AddManager(Manager managerInfo);
    }
}
