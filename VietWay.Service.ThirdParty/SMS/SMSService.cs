using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
namespace VietWay.Service.ThirdParty.SMS
{
    public class SMSService : ISMSService
    {
        private readonly string _token = Environment.GetEnvironmentVariable("SPEEDSMS_TOKEN")
            ?? throw new Exception("SPEEDSMS_TOKEN is not set in environment variables");
        private readonly string _deviceId = Environment.GetEnvironmentVariable("SPEEDSMS_DEVICE_ID")
            ?? throw new Exception("SPEEDSMS_DEVICE_ID is not set in environment variables");
        private readonly string _smsSendTokenMessage = Environment.GetEnvironmentVariable("SMS_SEND_TOKEN_MESSAGE")
            ?? throw new Exception("SMS_SEND_TOKEN_MESSAGE is not set in environment variables");
        public async Task<bool> SendOTP(string otp, string phoneNumber)
        {
            SpeedSMSAPI api = new(_token);
            string message = string.Format(_smsSendTokenMessage, otp);
            string[] phones = [phoneNumber];
            string result = await api.SendSMSAsync(phones, message, SpeedSMSAPI.TYPE_GATEWAY, _deviceId);
            if (result.Contains("\"status\":\"success\""))
            {
                return true;
            }
            return false;
        }
    }
}