using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Util.OtpUtil
{
    public class OtpGeneratorConfiguration
    {
        public int Length { get; set; }
        public int ExpiryTimeInMinute { get; set; }
    }
}
