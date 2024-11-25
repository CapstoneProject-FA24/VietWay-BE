using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class TourParticipantDTO
    {
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public required Gender Gender { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required decimal Price { get; set; }
    }
}
