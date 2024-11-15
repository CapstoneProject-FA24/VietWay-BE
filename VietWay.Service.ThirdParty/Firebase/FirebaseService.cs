using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Firebase
{
    public class FirebaseService : IFirebaseService
    {
        public async Task<string> GetEmailFromIdToken(string idToken)
        {
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            string email = decodedToken.Claims["email"].ToString();
            return email;
        }
    }
}
