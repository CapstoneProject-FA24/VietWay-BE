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
            ZaloPayRequest request = new ZaloPayRequest
            {
                Amount = (long)bookingPayment.Amount,
                EmbedData = "{}",
                Item = JsonConvert.SerializeObject(items, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }),
                Description = $"Thanh toan tour gia {bookingPayment.Amount}",
            };

            Random rnd = new Random();
            var param = new Dictionary<string, string>();

            var app_trans_id = rnd.Next(1000000); // Generate a random order's ID.
            string appTime = Utils.GetTimeStamp().ToString();
            string appTransId = _timeZoneHelper.GetUTC7Now().ToString("yyMMdd") + "_" + app_trans_id;

            param.Add("app_id", _zaloPayAppId);
            param.Add("app_user", _zaloPayAppUser);
            param.Add("app_time", appTime);
            param.Add("amount", request.Amount.ToString());
            param.Add("app_trans_id", appTransId); // mã giao dich có định dạng yyMMdd_xxxx
            param.Add("embed_data", request.EmbedData);
            param.Add("item", request.Item);
            param.Add("description", request.Description);
            param.Add("bank_code", "zalopayapp");

            var data = _zaloPayAppId + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" + param["amount"] + "|"
                + param["app_time"] + "|" + param["embed_data"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPayKey1, data));

            var result = await HttpHelper.PostFormAsync("https://sb-openapi.zalopay.vn/v2/create", param);
            return result["order_url"].ToString();
        }
    }
}
