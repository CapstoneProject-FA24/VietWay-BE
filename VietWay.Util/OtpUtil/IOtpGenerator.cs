using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Util.OtpUtil
{
    public interface IOtpGenerator
    {
        public string GenerateOtp();
        public TimeSpan GetOtpTimespan();
    }
}
