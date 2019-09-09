using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.IdentityServer
{
    public static class Config
    {

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>()
            {
               new ApiResource("WebApi","webApi.netcore"),
               new ApiResource("api1","My first api")
            };

        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
             {
               new IdentityResources.OpenId(),
               new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
            {
                ClientId="FakeClientId",
                ClientName="Fake Thired Application",
                AllowedGrantTypes= GrantTypes.Code,
                RequirePkce= true,
                RequireClientSecret= false,

                RedirectUris= { "http://localhost:5003/callback.html"},
                PostLogoutRedirectUris={ "http://localhost:5003/index.html"},
                AllowedCorsOrigins={ "http://localhost:5003"},

                AllowedScopes=
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "WebApi.NetCore"
                }
            },
                new Client()
                {
                    ClientId="client",
                    AllowedGrantTypes= GrantTypes.ClientCredentials,
                    ClientSecrets=
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes={ "api1"}
                },
                new Client()
                {
                    ClientId="user.client",
                    AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,
                    ClientSecrets=
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes={ "api1"}
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()
                {
                    SubjectId="111",
                    Username= "ZhangSan",
                    Password="password"
                },
                new TestUser()
                {
                    SubjectId="222",
                    Username="LiSi",
                    Password="password"
                }
            };
        }
    }
}
