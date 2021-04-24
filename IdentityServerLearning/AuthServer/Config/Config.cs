using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace AuthServer.Config
{
    public static class Config
    {
        #region Scopes
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("Garanti.Write","Garanti bankası yazma izni"),
                new ApiScope("Garanti.Read","Garanti bankası okuma izni"),
                new ApiScope("HalkBank.Write","HalkBank bankası yazma izni"),
                new ApiScope("HalkBank.Read","HalkBank bankası okuma izni"),
            };
        }
        #endregion

        #region Resources
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Garanti"){ Scopes = { "Garanti.Write", "Garanti.Read" } },
                new ApiResource("HalkBank"){ Scopes = { "HalkBank.Write", "HalkBank.Read" } }
            };
        }
        #endregion

        #region Clients
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "GarantiBankasi",
                    ClientName = "GarantiBankasi",
                    ClientSecrets = { new Secret("garanti".Sha256()) },
                    AllowedGrantTypes = { GrantType.ClientCredentials },
                    AllowedScopes = { "Garanti.Write", "Garanti.Read" }
                },
                new Client
                {
                    ClientId = "HalkBankasi",
                    ClientName = "HalkBankasi",
                    ClientSecrets = { new Secret("halkbank".Sha256()) },
                    AllowedGrantTypes = { GrantType.ClientCredentials },
                    AllowedScopes = { "HalkBank.Write", "HalkBank.Read" }
                },
                new Client
                {
                    ClientId = "OnlineBankamatik",
                    ClientName = "OnlineBankamatik",
                    ClientSecrets = { new Secret("onlinebankamatik".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile, 
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "Garanti.Write", 
                        "Garanti.Read" ,
                        "PositionAndAuthority",
                        "Roles",
                        "UserInfo"
                    },
                    RedirectUris = { "https://localhost:8000/signin-oidc" },
                    RequirePkce = false,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = 2 * 60 * 60 + (10 * 60),
                    RequireConsent = true
                    
                }
            };
        }
        #endregion

        public static IEnumerable<TestUser> GetTestUsers()
        {
            return new List<TestUser> {
                new TestUser {
                    SubjectId = "test-user1",
                    Username = "test-user1",
                    Password = "12345",
                    Claims = {
                        new Claim("name","test user1"),
                        new Claim("given_name","test user1 given"),
                        new Claim("website","https://wwww.testuser1.com"),
                        new Claim("gender","1"),
                        new Claim("position" , "Test User 1"),
                        new Claim("authority", "Test 1"),
                        new Claim("role", "admin")
                    }
                },
                new TestUser {
                    SubjectId = "test-user2",
                    Username = "test-user2",
                    Password = "12345",
                    Claims = {
                        new Claim("name","test user2"),
                        new Claim("website","https://wwww.testuser2.com"),
                        new Claim("gender","0"),
                        new Claim("position" , "Test User 2"),
                        new Claim("authority", "Test 2"),
                        new Claim("role", "moderator")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "PositionAndAuthority",
                    DisplayName = "Position And Authority",
                    Description = "Position and Authorization of User.",
                    UserClaims = { "position", "authority" }
                },
                 new IdentityResource {
                    Name = "Roles",
                    DisplayName = "Roles",
                    Description = "Kullanıcı rolleri",
                    UserClaims = { "role" }
                 },
                 new IdentityResource {
                    Name = "UserInfo",
                    DisplayName = "User Info",
                    Description = "Kullanıcı bilgileri",
                    UserClaims = { "name", "website" }
                 }

            };
        }

    }
}