using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Util.DateTimeUtil
{
    public interface ITimeZoneHelper
    {
        public DateTime GetUTC7Now();
        public DateTime GetLocalTimeFromUTC7(DateTime utc7Time);
    }
}
