using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.SMS
{
    public interface ISMSService
    {
        public Task<bool> SendOTP(string otp, string phoneNumber);
    }
}
