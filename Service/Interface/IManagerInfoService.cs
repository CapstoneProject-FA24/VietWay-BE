using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.ModelEntity;

namespace VietWay.Service.Interface
{
    public interface IManagerInfoService
    {
        public Task<List<ManagerInfo>> GetAllManagerInfos(int pageSize, int pageIndex);
        public Task<ManagerInfo?> GetManagerInfoById(int id);
        public Task<ManagerInfo> EditManagerInfo(ManagerInfo managerInfo);
        public Task<ManagerInfo> AddManager(ManagerInfo managerInfo);
    }
}
