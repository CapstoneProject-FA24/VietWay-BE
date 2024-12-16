using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Customer.Configuration
{
    public class TourReviewServiceConfiguration(int reviewTourExpireAfterDays)
    {
        private readonly int _reviewTourExpireAfterDays = reviewTourExpireAfterDays;
        public int ReviewTourExpireAfterDays { get => _reviewTourExpireAfterDays; }
    }
}
