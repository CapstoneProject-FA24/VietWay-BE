using Microsoft.EntityFrameworkCore;
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
        public async Task<(int totalCount, List<Manager> items)> GetAllManagerInfos(int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .ManagerRepository
                .Query();
            int count = await query.CountAsync();
            List<Manager> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Account)
                .ToListAsync();
            return (count, items);
        }
        public async Task<Manager?> GetManagerInfoById(string id)
        {
            return await _unitOfWork
                .ManagerRepository
                .Query()
                .Include(x => x.Account)
                .SingleOrDefaultAsync(x => x.ManagerId.Equals(id));
        }
        public async Task<Manager> EditManagerInfo(Manager managerInfo)
        {
            await _unitOfWork.ManagerRepository
                .UpdateAsync(managerInfo);
            return managerInfo;
        }

        public async Task<Manager> AddManager(Manager managerInfo)
        {
            await _unitOfWork.ManagerRepository
                .CreateAsync(managerInfo);
            return managerInfo;
        }
    }
}
