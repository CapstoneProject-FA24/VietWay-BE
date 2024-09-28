using Microsoft.EntityFrameworkCore;
using Repository.ModelEntity;
using Repository.UnitOfWork;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implement
{
    public class ManagerInfoService : IManagerInfoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManagerInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ManagerInfo>> GetAllManagerInfos(int pageSize, int pageIndex)
        {
            return await _unitOfWork.ManagerInfoRepository
                .Query()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Account)
                .ToListAsync();
        }
        public async Task<ManagerInfo?> GetManagerInfoById(int id)
        {
            return await _unitOfWork.ManagerInfoRepository
                .Query()
                .Where(x => x.ManagerId.Equals(id))
                .Include(x => x.Account)
                .FirstOrDefaultAsync();
        }
        public async Task<ManagerInfo> EditManagerInfo(ManagerInfo managerInfo)
        {
            await _unitOfWork.ManagerInfoRepository
                .Update(managerInfo);
            return managerInfo;    
        }

        public async Task<ManagerInfo> AddManager(ManagerInfo managerInfo)
        {
            await _unitOfWork.ManagerInfoRepository
                .Create(managerInfo);
            return managerInfo;
        }
    }
}
