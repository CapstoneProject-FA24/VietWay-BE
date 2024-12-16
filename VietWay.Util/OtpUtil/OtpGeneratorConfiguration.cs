using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Util.OtpUtil
{
    public class OtpGeneratorConfiguration(int length, int expiryTimeInMinute)
    {
        private readonly int _length = length;
        private readonly int _expiryTimeInMinute = expiryTimeInMinute;
        public int Length { get => _length; }
        public int ExpiryTimeInMinute { get => _expiryTimeInMinute; }
    }
}
