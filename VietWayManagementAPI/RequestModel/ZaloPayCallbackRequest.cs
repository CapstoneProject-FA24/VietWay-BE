using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Management.RequestModel
{
    public class ZaloPayCallbackRequest
    {
        [Required]
        [FromQuery(Name = "appid")]
        public int AppId { get; set; }
        [Required]
        [FromQuery(Name = "apptransid")]
        public string AppTransId { get; set; }
    }
}
