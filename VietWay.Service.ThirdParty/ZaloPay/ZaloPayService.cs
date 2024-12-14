using IdGen;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Service.ThirdParty.Twitter;
using VietWay.Util.DateTimeUtil;
using ZaloPay.Helper;
using ZaloPay.Helper.Crypto;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static System.Net.WebRequestMethods;

namespace VietWay.Service.ThirdParty.ZaloPay
{
    public class ZaloPayService(ITimeZoneHelper timeZoneHelper, ZaloPayServiceConfiguration config) : IZaloPayService
    {
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public readonly string _zaloPayAppId = config.ZaloPayAppId;
        public readonly string _zaloPayAppUser = config.ZaloPayAppUser;
        public readonly string _zaloPayKey1 = config.ZaloPayKey1;
        public readonly string _zaloPayKey2 = config.ZaloPayKey2;
        public async Task<string> GetPaymentUrl(BookingPayment bookingPayment)
        {
            List<BookingItem> items = new List<BookingItem>
            {
                new BookingItem
                {
                    BookingId = bookingPayment.BookingId,
                    TourId = bookingPayment.Booking.TourId,
                    ContactFullName = bookingPayment.Booking.ContactFullName,
                    CustomerId = bookingPayment.Booking.CustomerId,
                    CreatedAt = bookingPayment.Booking.CreatedAt,
                    NumberOfParticipants = bookingPayment.Booking.NumberOfParticipants,
                    TotalPrice = bookingPayment.Booking.TotalPrice
                }
            };
            string returnUrl = "";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                returnUrl = "http://localhost:5173/dat-tour/thanh-toan/hoan-thanh/" + bookingPayment.BookingId;
            }
            else
            {
                returnUrl = "https://vietway.projectpioneer.id.vn/dat-tour/thanh-toan/hoan-thanh/" + bookingPayment.BookingId;
            }
            ZaloPayRequest request = new ZaloPayRequest
            {
                Amount = (long)bookingPayment.Amount,
                EmbedData = "{\"redirecturl\": \"" + returnUrl + "\"}",
                Item = JsonConvert.SerializeObject(items, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }),
                Description = $"Thanh toan tour gia {bookingPayment.Amount}",
            };

            var param = new Dictionary<string, string>();

            param.Add("app_id", _zaloPayAppId);
            param.Add("app_user", _zaloPayAppUser);
            param.Add("app_time", Utils.GetTimeStamp().ToString());
            param.Add("amount", request.Amount.ToString());
            param.Add("app_trans_id", DateTime.Now.ToString("yyMMddhhmmss") + "_" + bookingPayment.PaymentId.ToString());
            param.Add("embed_data", request.EmbedData);
            param.Add("item", request.Item);
            param.Add("description", request.Description);
            param.Add("bank_code", "zalopayapp");
            param.Add("callback_url", "https://api.vietway.projectpioneer.id.vn/api/booking-payments/ZaloPayCallback");

            var data = _zaloPayAppId + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" + param["amount"] + "|"
                + param["app_time"] + "|" + param["embed_data"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPayKey1, data));

            var result = await HttpHelper.PostFormAsync("https://sb-openapi.zalopay.vn/v2/create", param);
            return result["order_url"].ToString();
        }

        public async Task<ZaloPayResponse> VerifyTransactionLocal(ZaloPayCallback zaloPayCallback)
        {
            var param = new Dictionary<string, string>();
            param.Add("app_id", zaloPayCallback.AppId.ToString());
            param.Add("app_trans_id", zaloPayCallback.AppTransId);
            var data = zaloPayCallback.AppId + "|" + zaloPayCallback.AppTransId + "|" + _zaloPayKey1;

            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPayKey1, data));
            var result = await HttpHelper.PostFormAsync("https://sb-openapi.zalopay.vn/v2/query", param);
            ZaloPayResponse response = new ZaloPayResponse
            {
                ReturnCode = int.Parse(result["return_code"].ToString()),
                ReturnMessage = result["return_message"].ToString(),
                SubReturnCode = int.Parse(result["sub_return_code"].ToString()),
                SubReturnMessage = result["sub_return_message"].ToString(),
                IsProcessing = bool.Parse(result["is_processing"].ToString()),
                Amount = long.Parse(result["amount"].ToString()),
                DiscountAmount = long.Parse(result["discount_amount"].ToString()),
                ZpTransId = long.Parse(result["zp_trans_id"].ToString()),
                ServerTime = long.Parse(result["server_time"].ToString())
            };
            return response;
        }

        public Dictionary<string, object> VerifyTransaction(CallbackData data)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataStr = Convert.ToString(data.Data);
                var reqMac = Convert.ToString(data.Mac);

                var mac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPayKey2, dataStr);

                if (!reqMac.Equals(mac))
                {
                    result["return_code"] = -1;
                    result["return_message"] = "mac not equal";
                }
                else
                {
                    result["return_code"] = 1;
                    result["return_message"] = "success";
                }
            }
            catch (Exception ex)
            {
                result["return_code"] = 0; // ZaloPay server sẽ callback lại (tối đa 3 lần)
                result["return_message"] = ex.Message;
            }
            return result;
        }
    }
}
