using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IEntityHistoryService
    {
        Task<List<EntityHistoryDTO>> GetEntityHistoryAsync(string entityId, EntityType entityType);
    }
}
