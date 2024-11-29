using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;
using Newtonsoft.Json;

namespace VietWay.Service.ThirdParty.ZaloPay
{
    public class ZaloPayRequest
    {
        public long Amount { get; set; }
        public string EmbedData { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
    }

    public class BookingItem
    {
        public string? BookingId { get; set; }
        public string? TourId { get; set; }
        public string? CustomerId { get; set; }
        public int NumberOfParticipants { get; set; }
        public string? ContactFullName { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ZaloPayCallback
    {
        public int AppId { get; set; }
        public string AppTransId { get; set; }
    }

    public class CallbackData
    {
        [JsonProperty("data")]
        public string Data { get; set; }
        [JsonProperty("mac")]
        public string Mac { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
    }

    public class CallbackDataContent
    {
        [JsonProperty("app_trans_id")]
        public string AppTransId { get; set; }

        [JsonProperty("server_time")]
        public long ServerTime { get; set; }

        [JsonProperty("zp_trans_id")]
        public long ZpTransId { get; set; }
    }
}
