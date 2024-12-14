using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class ScheduleDTO
    {
        public int DayNumber { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required List<AttractionPreviewDTO> Attractions { get; set; }
    }
}
