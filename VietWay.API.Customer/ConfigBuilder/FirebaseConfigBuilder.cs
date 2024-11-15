using System.Net;

public static class FirebaseConfigBuilder
{
    public static string BuildConfig()
    {
        string projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID")
            ?? throw new Exception("FIREBASE_PROJECT_ID is not set in environment variables");
        string privateKeyId = Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY_ID")
            ?? throw new Exception("FIREBASE_PRIVATE_KEY_ID is not set in environment variables");
        string privateKey = Environment.GetEnvironmentVariable("FIREBASE_PRIVATE_KEY")
            ?? throw new Exception("FIREBASE_PRIVATE_KEYis not set in environment variables");
        string clientEmail = Environment.GetEnvironmentVariable("FIREBASE_CLIENT_EMAIL")
            ?? throw new Exception("FIREBASE_CLIENT_EMAIL is not set in environment variables");
        string clientId = Environment.GetEnvironmentVariable("FIREBASE_CLIENT_ID")
            ?? throw new Exception("FIREBASE_CLIENT_ID is not set in environment variables");

        string encodedClientEmail = WebUtility.UrlEncode(clientEmail);

        return $@"
        {{
            ""type"": ""service_account"",
            ""project_id"": ""{projectId}"",
            ""private_key_id"": ""{privateKeyId}"",
            ""private_key"": ""{privateKey}"",
            ""client_email"": ""{clientEmail}"",
            ""client_id"": ""{clientId}"",
            ""auth_uri"": ""https://accounts.google.com/o/oauth2/aut"",
            ""token_uri"": ""https://oauth2.googleapis.com/token"",
            ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
            ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/{encodedClientEmail}"",
            ""universe_domain"": ""googleapis.com""
        }}";
    }
}