using Dbank.Digisoft.Church.Web.Models;
using Dbank.Digisoft.Church.Web.Services.Authorization;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace Dbank.Digisoft.Church.Web.Services
{
    public class ChurchMemberService
    {
        public readonly ILogger<ChurchMemberService> _logger;
        public readonly ITokenService _tokenSvc;

        public ChurchMemberService(ILogger<ChurchMemberService> logger, ITokenService tokenSvc)
        {
            _logger = logger;
            _tokenSvc = tokenSvc;
        }

        public async Task<List<ChurchMember>?> GetChurchMembers()
        {
            using var client = new HttpClient();
            var tokenResponse = await _tokenSvc.GetToken("churchapi.read");
            client.SetBearerToken(tokenResponse.AccessToken);
            var result = await client.GetAsync("member/all");
            if (!result.IsSuccessStatusCode) return null;
            var model = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ChurchMember>>(model);
        }
    }
}
