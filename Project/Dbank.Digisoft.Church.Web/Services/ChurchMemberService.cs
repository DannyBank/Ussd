using Dbank.Digisoft.Church.Web.Services.Authorization;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace Dbank.Digisoft.Church.Web.Services
{
    public class ChurchMemberService
    {
        public readonly ILogger<ChurchMemberService> _logger;
        public readonly ITokenService _tokenSvc;
        public readonly HttpClient client;

        public ChurchMemberService(ILogger<ChurchMemberService> logger, ITokenService tokenSvc,
            IConfiguration config)
        {
            _logger = logger;
            _tokenSvc = tokenSvc;
            client = new() { BaseAddress = new Uri(config.GetConnectionString("Api")) };
        }

        public async Task<List<Ussd.Data.Models.ChurchModels.Church>?> GetChurches()
        {
            var tokenResponse = await _tokenSvc.GetToken("churchapi.read");
            client.SetBearerToken(tokenResponse.AccessToken);
            var result = await client.GetAsync($"api/church/getall");
            if (!result.IsSuccessStatusCode) return null;
            var model = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Ussd.Data.Models.ChurchModels.Church>>(model);
        }
    }
}
