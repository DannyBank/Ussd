using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Dbank.Digisoft.Church.Web.Services.Authorization
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly IOptions<IdentityServerSettings> _options;
        private readonly DiscoveryDocumentResponse _discoveryDoc;

        public TokenService(IOptions<IdentityServerSettings> options, ILogger<TokenService> logger)
        {
            _logger = logger;
            _options = options;
            using var httpClient = new HttpClient();
            _discoveryDoc = httpClient.GetDiscoveryDocumentAsync(_options.Value.DiscoveryUrl).Result;
            if (_discoveryDoc.IsError)
            {
                _logger.LogError("Discovery Document was not found: {error}", _discoveryDoc.Error);
                throw new Exception($"Discovery Document was not found: {_discoveryDoc.Error}");
            }
        }

        public async Task<TokenResponse> GetToken(string scope)
        {
            using var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _discoveryDoc.TokenEndpoint,
                ClientId = _options.Value.ClientName,
                ClientSecret = _options.Value.ClientPassword,
                Scope = scope
            });
            if (tokenResponse.IsError)
            {
                _logger.LogError("Unable to get token. Error: {err}", tokenResponse.Error);
                throw new Exception("Unable to get token", tokenResponse.Exception);
            }
            return tokenResponse;
        }
    }
}
