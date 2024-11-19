using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
namespace VietWay.Service.ThirdParty.Sms
{
    public class SmsService(SmsConfiguration config) : ISmsService
    {
        private readonly string _token = config.Token;
        private readonly string _deviceId = config.DeviceId;
        private readonly string _smsSendTokenMessage = config.SendTokenMessage;
        public async Task<bool> SendOTP(string otp, string phoneNumber)
        {
            SpeedSmsApi api = new(_token);
            string message = string.Format(_smsSendTokenMessage, otp);
            string[] phones = [phoneNumber];
            string result = await api.SendSMSAsync(phones, message, SpeedSmsApi.TYPE_GATEWAY, _deviceId);
            if (result.Contains("\"status\":\"success\""))
            {
                return true;
            }
            return false;
        }
    }
}