namespace Authentication.Options;

public class JwtOptions
{
    public const string SettingsKey = "Jwt";

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string SecurityKey { get; set; } = string.Empty;

    public int TokenExpirationInMinutes { get; set; }
}
