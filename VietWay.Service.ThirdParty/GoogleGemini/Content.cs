using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.GoogleGemini
{
    public class Content
    {
        public string? Role { get; set; }
        public required List<Part> Parts { get; set; }
    }
}
