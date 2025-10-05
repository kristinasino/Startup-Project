namespace Infrastructure.Configuration
{
    public class TokenSettings
    {
        public string Key { get; set; }
        public int TokenTimeoutMinutes { get; set; }
        public int RefreshTokenTimeoutDays { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
        public int ReportTokenTimeout { get; set; }
    }
}