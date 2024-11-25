using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Sms
{
    public interface ISmsService
    {
        public Task<bool> SendOTP(string otp, string phoneNumber);
    }
}
