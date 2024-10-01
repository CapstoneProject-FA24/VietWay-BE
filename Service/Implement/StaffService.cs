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
    public class StaffService: IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StaffService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Staff> AddStaff(Staff staffInfo)
        {
            await _unitOfWork.StaffRepository
                .Create(staffInfo);
            return staffInfo;
        }

        public async Task<Staff> EditStaffInfo(Staff staffInfo)
        {
            await _unitOfWork.StaffRepository
                .Update(staffInfo);
            return staffInfo;
        }

        public async Task<List<Staff>> GetAllStaffInfos(int pageSize, int pageIndex)
        {
            return await _unitOfWork.StaffRepository
                .Query()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Account)
                .ToListAsync();
        }

        public async Task<Staff?> GetStaffInfoById(int id)
        {
            return await _unitOfWork.StaffRepository
                .Query()
                .Where(x => x.StaffId.Equals(id))
                .Include(x => x.Account)
                .FirstOrDefaultAsync();
        }
    }
}
