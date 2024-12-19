using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Util.DateTimeUtil
{
    public class TimeZoneHelper : ITimeZoneHelper
    {
        public DateTime GetUTC7Now()
        {
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh"));
        }
    }
}
