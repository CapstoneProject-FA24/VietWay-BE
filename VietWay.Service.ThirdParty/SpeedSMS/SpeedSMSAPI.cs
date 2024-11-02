using System.Net;
using System.Text;
using System.Text.Json;

namespace VietWay.Service.ThirdParty.SpeedSMS
{
    internal class SpeedSMSAPI(string token)
    {
        public const int TYPE_QC = 1;
        public const int TYPE_CSKH = 2;
        public const int TYPE_BRANDNAME = 3;
        public const int TYPE_BRANDNAME_NOTIFY = 4; // Gửi sms sử dụng brandname Notify
        public const int TYPE_GATEWAY = 5; // Gửi sms sử dụng app android từ số di động cá nhân, download app tại đây: https://speedsms.vn/sms-gateway-service/

        private const string _rootUrl = "https://api.speedsms.vn/index.php";
        private readonly string _accessToken = token;

        private static readonly JsonSerializerOptions s_camelCase = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private static string EncodeNonAsciiCharacters(string value)
        {
            StringBuilder sb = new();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public async Task<string> GetUserInfoAsync()
        {
            string url = $"{_rootUrl}/user/info";
            NetworkCredential myCreds = new(_accessToken, ":x");

            using HttpClientHandler handler = new() { Credentials = myCreds };
            using HttpClient client = new(handler);
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Throws an exception if the HTTP response is an error code

            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }


        public async Task<string> SendSMSAsync(string[] phones, string content, int type, string sender)
        {
            string url = $"{_rootUrl}/sms/send";

            if (phones.Length <= 0 ||
                string.IsNullOrWhiteSpace(content) ||
                (type == TYPE_BRANDNAME && string.IsNullOrWhiteSpace(sender)))
            {
                return string.Empty;
            }
            NetworkCredential myCreds = new(_accessToken, ":x");
            using HttpClientHandler handler = new() { Credentials = myCreds };
            using HttpClient client = new(handler);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var payload = new
            {
                To = phones,
                Content = content,
                Type = type,
                Sender = sender
            };
            string jsonPayload = JsonSerializer.Serialize(payload, s_camelCase);
            HttpContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> SendMMSAsync(string[] phones, string content, string link, string sender)
        {
            string url = $"{_rootUrl}/mms/send";
            if (phones.Length <= 0 || string.IsNullOrWhiteSpace(content))
            {
                return string.Empty;
            }
            NetworkCredential myCreds = new(_accessToken, ":x");

            using HttpClientHandler handler = new() { Credentials = myCreds };
            using HttpClient client = new(handler);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var payload = new
            {
                To = phones,
                Content = content,
                Link = link,
                Sender = sender
            };
            string jsonPayload = JsonSerializer.Serialize(payload, s_camelCase);

            HttpContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    }
}
