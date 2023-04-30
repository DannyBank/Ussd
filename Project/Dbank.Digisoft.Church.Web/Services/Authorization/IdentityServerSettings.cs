namespace Dbank.Digisoft.Church.Web.Services.Authorization
{
    public class IdentityServerSettings
    {
        public string DiscoveryUrl { get; set; } = null!;
        public string ClientName { get; set; } = null!;
        public string ClientPassword { get; set; } = null!;
        public bool UseHttps { get; set; }
    }
}
