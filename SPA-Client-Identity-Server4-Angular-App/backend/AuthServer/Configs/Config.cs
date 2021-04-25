using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthServer.Configs
{
    static public class Config
    {
        #region Scoped
        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope("Garanti.Write","Garanti bankası yazma izni."),
                new ApiScope("Garanti.Read","Garanti bankası okuma izni.")
            };
        #endregion
        #region Resources
        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("Garanti"){ Scopes = {"Garanti.Write", "Garanti.Read" } }
            };
        #endregion
        #region Clients
        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "AngularClient",
                    ClientName = "Angular Client",
                    RequireClientSecret = false,
                    AllowedScopes = {
                        "Garanti.Write",
                        "Garanti.Read",
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        "Roles"
                    },
                    RedirectUris = {"http://localhost:4200/callback", "http://localhost:4200/silent-callback"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    PostLogoutRedirectUris = {"http://localhost:4200"},
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AccessTokenLifetime = 70
                }
            };
        #endregion
        #region Users
        public static IEnumerable<TestUser> GetTestUsers() =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "test-user1",
                    Username = "test-user1",
                    Password = "12345",
                    Claims =
                    {
                        new Claim(JwtRegisteredClaimNames.Email, "test-user@gmail.com"),
                        new Claim("role", "admin"),
                        new Claim("authority","true")
                    }
                }
            };
        #endregion
        #region Identity Resources
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "Roles",
                    DisplayName = "Roles",
                    Description = "User roles",
                    UserClaims = {"role", "authority"}
                }
            };
        #endregion
    }
}
