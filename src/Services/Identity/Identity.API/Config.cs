using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace Identity.API
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                //new Client()
                //{
                //    ClientId = "consumerclient1",
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    AllowedScopes = { "consumerapi" }
                //},
                 new Client
                   {
                       ClientId = "consumerclient",
                       ClientName = "consumer",
                       AllowedGrantTypes = GrantTypes.Code,
                       RequirePkce = false,
                       AllowRememberConsent = false,
                       RedirectUris = new List<string>()
                       {
                           "https://localhost:5014/signin-oidc"
                       },
                       PostLogoutRedirectUris = new List<string>()
                       {
                           "https://localhost:5014/signout-callback-oidc"
                       },
                       ClientSecrets = new List<Secret>
                       {
                           new Secret("secret".Sha256())
                       },
                       AllowedScopes = new List<string>
                       {
                           IdentityServerConstants.StandardScopes.OpenId,
                           IdentityServerConstants.StandardScopes.Profile,
                       }
                   }
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
    {
        new ApiScope("consumerapi", "Consumer Api")
    };
        public static IEnumerable<ApiResource> ApiResources =>
    new ApiResource[]
    {

    };
        public static IEnumerable<IdentityResource> IdentityResources =>
    new IdentityResource[]
    {
                      new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
    };
        public static List<TestUser> TestUsers =>
    new()
    {
        new TestUser
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "raza",
                    Password = "123456",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "hashim"),
                        new Claim(JwtClaimTypes.FamilyName, "raza")
                    }
                }
    };

    }
}
