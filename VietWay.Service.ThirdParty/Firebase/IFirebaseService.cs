using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Firebase
{
    public interface IFirebaseService
    {
        public Task<string> GetEmailFromIdToken(string idToken);
    }
}
