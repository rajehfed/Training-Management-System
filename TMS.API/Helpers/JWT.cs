namespace TMS.API.Helpers;

public class JWT
{
    public string Secret { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string ExpirationMinutes { get; set; } = null!;
}
