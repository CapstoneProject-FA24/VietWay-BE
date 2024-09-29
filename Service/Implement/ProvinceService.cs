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
    public class ProvinceService(IUnitOfWork unitOfWork) : IProvinceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public async Task<List<Province>> GetAllProvince()
        {
            return await _unitOfWork
                .ProvinceRepository
                .Query()
                .ToListAsync();
        }

        public Task<Province?> GetProvinceById(long id)
        {
            return _unitOfWork
                .ProvinceRepository
                .Query()
                .SingleOrDefaultAsync(x => x.ProvinceId == id);
        }
    }
}
