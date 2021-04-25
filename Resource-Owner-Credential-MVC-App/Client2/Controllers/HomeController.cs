using Client2.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task Login(UserLogin userLogin)
        {
            HttpClient client = new HttpClient();
            DiscoveryDocumentResponse disco = await client.GetDiscoveryDocumentAsync("https://localhost:1000");
            PasswordTokenRequest password = new PasswordTokenRequest()
            {
                UserName = userLogin.Username,
                Password = userLogin.Password,
                ClientId = "ResourceOwnerClient",
                ClientSecret = "resourceownerclient",
                Address = disco.TokenEndpoint
            };
            TokenResponse token = await client.RequestPasswordTokenAsync(password);
            UserInfoRequest userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            };
            UserInfoResponse userInfo = await client.GetUserInfoAsync(userInfoRequest);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            AuthenticationProperties properties = new AuthenticationProperties();
            properties.StoreTokens(new List<AuthenticationToken>
            {
              new AuthenticationToken
                                     {
                                         Name = OpenIdConnectParameterNames.IdToken,
                                         Value = token.IdentityToken
                                     },
              new AuthenticationToken
                                     {
                                         Name = OpenIdConnectParameterNames.AccessToken,
                                         Value = token.AccessToken
                                     },
              new AuthenticationToken
                                     {
                                         Name = OpenIdConnectParameterNames.RefreshToken,
                                         Value = token.RefreshToken
                                     },
              new AuthenticationToken
                                     {
                                         Name = OpenIdConnectParameterNames.ExpiresIn,
                                         Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("O")
                                     },
            });
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, properties);
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync();
        }

    }
}
