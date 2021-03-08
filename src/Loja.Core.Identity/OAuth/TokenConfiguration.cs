namespace Loja.Core.Identity.OAuth
{
    public class TokenConfiguration
    {
        public string[] Audiences { get; set; }
        public byte ClockSkew { get; set; }
        public double ExpirationInMinutesAccessToken { get; set; }
        public double ExpirationInMinutesRefreshToken { get; set; }
        public string Issuer { get; set; }
        public string PublicKeyUrl { get; set; }
        public bool RequiredHttpsMetadata { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public bool ValidateLifeTime { get; set; }
        public bool ValidateTime { get; set; }
    }
}