namespace Blog;

public static class Configuration
{
    public static string? JwtKey { get; set; }
    public static string? ApiKeyName { get; set; }
    public static string? ApiKey { get; set; }
    public static SmtpConfiguration Smtp { get; set; } = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}