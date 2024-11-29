using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.Service.Management.Implement
{
    public class EntityHistoryService(IUnitOfWork unitOfWork) : IEntityHistoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<EntityHistoryDTO>> GetEntityHistoryAsync(string entityId, EntityType entityType)
        {
            return await _unitOfWork.EntityHistoryRepository.Query()
                .Where(x => x.EntityId == entityId && x.EntityType == entityType)
                .Select(x => new EntityHistoryDTO()
                {
                    Action = x.Action,
                    ModifiedBy = x.ModifiedBy,
                    ModifierRole = x.ModifierRole,
                    NewStatus = x.StatusHistory.NewStatus,
                    OldStatus = x.StatusHistory.OldStatus,
                    Reason = x.Reason,
                    Timestamp = x.Timestamp,
                    ModifierName =  x.Modifier.Role == UserRole.Customer ? x.Modifier.Customer.FullName : 
                                    x.Modifier.Role == UserRole.Staff ? x.Modifier.Staff.FullName : 
                                    x.Modifier.Role == UserRole.Manager ? x.Modifier.Manager.FullName : null
                }).ToListAsync();


        }
    }
}
