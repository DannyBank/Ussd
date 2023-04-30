
using IdentityModel.Client;

namespace Dbank.Digisoft.Church.Web.Services.Authorization
{
    public interface ITokenService
    {
        Task<TokenResponse> GetToken(string scope);
    }
}
