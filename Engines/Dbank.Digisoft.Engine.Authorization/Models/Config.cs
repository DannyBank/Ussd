using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace Dbank.Digisoft.Engine.Authorization.Models
{
    public static class Config
    {
        public static List<TestUser> TestUsers
        {
            get
            {
                return new List<TestUser>
                {
                    new TestUser()
                    {
                        SubjectId = "818727",
                        Username = "Dbank",
                        Password = "D@@nn33ll123",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Daniel Ankrah"),
                            new Claim(JwtClaimTypes.GivenName, "Daniel"),
                            new Claim(JwtClaimTypes.FamilyName, "Ankrah"),
                            new Claim(JwtClaimTypes.Email, "dbankdigisoft@gmail.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true"),
                            new Claim(JwtClaimTypes.Role, "admin"),
                            new Claim(JwtClaimTypes.WebSite, "http://dbankdigisoft.com")
                        }
                    },
                    new TestUser()
                    {
                        SubjectId = "818728",
                        Username = "Nana",
                        Password = "Agy3m@n",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Nana Agyeman"),
                            new Claim(JwtClaimTypes.GivenName, "Kwesi"),
                            new Claim(JwtClaimTypes.FamilyName, "Agyeman"),
                            new Claim(JwtClaimTypes.Email, "nanakwesi@gmail.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true"),
                            new Claim(JwtClaimTypes.Role, "user"),
                            new Claim(JwtClaimTypes.WebSite, "http://consociate.com")
                        }
                    }
                };
            }
        }

        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                new ApiScope("weatherapi.read"),
                new ApiScope("weatherapi.write")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource("weatherapi")
                {
                    Scopes = new List<string>{ "weatherapi.read", "weatherapi.write"},
                    ApiSecrets = new List<Secret>{ new Secret("ScopeSecret".Sha256())},
                    UserClaims = new List<string>{"role"}
                }
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientId = "m2m.client",//machine t0 machine clients
                    ClientName = "Client Credentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("SuperSecretPassword".Sha256())},
                    AllowedScopes = {"weatherapi.read","weatherapi.write"}
                },
                new Client
                {
                    ClientId = "interactive",//web clients
                    ClientSecrets = {new Secret()},
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"https://localhost:55334/signin-oidc"},
                    FrontChannelLogoutUri = "https://localhost:55334/signout-oidc",
                    PostLogoutRedirectUris = {"https://localhost:55334/signout-callback-oidc"},
                    AllowOfflineAccess = true,
                    AllowedScopes = {"openid", "profile", "weatherapi.read"},
                }
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string>{"role"}
                }
            };
    }
}
