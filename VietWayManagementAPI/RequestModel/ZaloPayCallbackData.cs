using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VietWay.API.Management.RequestModel
{
    public class ZaloPayCallbackData
    {
        public string Data { get; set; }
        public string Mac { get; set; }
        public int Type { get; set; }
    }
}
