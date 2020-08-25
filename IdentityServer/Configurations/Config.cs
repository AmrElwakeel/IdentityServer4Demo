using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Configurations
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                    new ApiScope("api", "My API"),
            };

        public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,


                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                AllowedScopes = { "api" }
            },
            new Client
            {
                ClientId = "client_mvc",
                ClientSecrets = { new Secret("client_mvc_secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,
                 
                RedirectUris = { "https://localhost:44334/signin-oidc" },
                 
                PostLogoutRedirectUris = { "https://localhost:44334/signout-callback-oidc" },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api"
                },
                AllowOfflineAccess=true,
                RequireConsent=false
                
            }
        };
    }
}
