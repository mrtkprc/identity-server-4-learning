using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PasswordOwnerCredentialExample.Configs
{
    public class Config
    {
        static public IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new("Garanti.Write","Garanti bankası yazma izni"),
                new("Garanti.Read","Garanti bankası okuma izni"),
            };
        static public IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new("Garanti") {Scopes = { "Garanti.Write", "Garanti.Read"} }
            };
        static public IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client{
                    ClientId = "Hybrid",
                    ClientName = "Hybrid",
                    ClientSecrets = { new Secret("hybrid".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes =
                    {
                        "Garanti.Write", "Garanti.Read",
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    },
                    AccessTokenLifetime = 3 * 60,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (3 * 60) + 30  ,
                    RedirectUris = { "https://localhost:3000/signin-oidc" },
                    RequirePkce = false
                },
                new Client{
                    ClientId = "ResourceOwnerClient",
                    ClientName = "Resource Owner Client",
                    ClientSecrets = { new Secret("resourceownerclient".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        "Garanti.Write", "Garanti.Read",
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        "Roles"
                    },
                    AccessTokenLifetime = 3 * 60,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (3 * 60) + 30,
                    RequirePkce = false
                }
            };
        static public IEnumerable<TestUser> GetTestUsers() =>
            new List<TestUser>
            {
                new TestUser
                {
                     SubjectId = "test-user1",
                     Username = "test-user1",
                     Password = "12345",
                     Claims = new List<Claim>
                     {
                         new Claim("email", "test-user@identity.com"),
                         new Claim("role", "admin"),
                         new Claim("role", "moderator"),
                         new Claim("name", "hilmi")
                     }
                }
            };
        static public IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "Roles",
                    Description = "Roles",
                    DisplayName = "Roles",
                    UserClaims =
                    {
                        "role"
                    }
                }
            };
    }
}
