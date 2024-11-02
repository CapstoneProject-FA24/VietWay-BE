using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface ITourDurationService
    {
        public Task<List<TourDurationDTO>> GetTourDurationsAsync();
    }
}
