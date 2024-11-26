using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Util.DateTimeUtil
{
    public class TimeZoneHelper : ITimeZoneHelper
    {
        public DateTime GetLocalTimeFromUTC7(DateTime utc7Time)
        {
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(utc7Time, TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh"));
            return TimeZoneInfo.ConvertTime(utcTime, TimeZoneInfo.Local);
        }

        public DateTime GetUTC7Now()
        {
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh"));
        }
    }
}
