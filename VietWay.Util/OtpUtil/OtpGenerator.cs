using System.Security.Cryptography;

namespace VietWay.Util.OtpUtil
{
    public class OtpGenerator(OtpGeneratorConfiguration config) : IOtpGenerator
    {
        private readonly int _length = config.Length;
        private readonly int _expiryTimeInMinute = config.ExpiryTimeInMinute;
        public string GenerateOtp()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[_length];
            rng.GetBytes(randomBytes);

            string otp = string.Empty;
            foreach (byte b in randomBytes)
            {
                otp += (b % 10).ToString();
            }
            return otp;
        }

        public TimeSpan GetOtpTimespan()
        {
            return TimeSpan.FromMinutes(_expiryTimeInMinute);
        }
    }
}
