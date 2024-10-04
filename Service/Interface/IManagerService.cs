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
        public Task<(int totalCount, List<Manager> items)> GetAllManagerInfos(int pageSize, int pageIndex);
        public Task<Manager?> GetManagerInfoById(string id);
        public Task<Manager> EditManagerInfo(Manager managerInfo);
        public Task<Manager> AddManager(Manager managerInfo);
    }
}
