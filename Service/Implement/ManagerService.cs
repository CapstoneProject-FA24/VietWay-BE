using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class ManagerService : IManagerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManagerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Manager>> GetAllManagerInfos(int pageSize, int pageIndex)
        {
            return await _unitOfWork.ManagerInfoRepository
                .Query()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Account)
                .ToListAsync();
        }
        public async Task<Manager?> GetManagerInfoById(int id)
        {
            return await _unitOfWork.ManagerInfoRepository
                .Query()
                .Where(x => x.ManagerId.Equals(id))
                .Include(x => x.Account)
                .FirstOrDefaultAsync();
        }
        public async Task<Manager> EditManagerInfo(Manager managerInfo)
        {
            await _unitOfWork.ManagerInfoRepository
                .Update(managerInfo);
            return managerInfo;
        }

        public async Task<Manager> AddManager(Manager managerInfo)
        {
            await _unitOfWork.ManagerInfoRepository
                .Create(managerInfo);
            return managerInfo;
        }
    }
}
