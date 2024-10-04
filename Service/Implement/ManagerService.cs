﻿using Microsoft.EntityFrameworkCore;
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
        public async Task<(int totalCount, List<Manager> items)> GetAllManagerInfos(int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .ManagerInfoRepository
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
                .ManagerInfoRepository
                .Query()
                .Include(x => x.Account)
                .SingleOrDefaultAsync(x => x.ManagerId.Equals(id));
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
