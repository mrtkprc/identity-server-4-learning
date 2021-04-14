using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;

namespace HalkbankAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class HalkBankController : ControllerBase
    {
        [HttpGet("{musteriID}")]
        public double Bakiye(int musteriId)
        {
            //....
            return 500.15;
        }
        [HttpGet("{musteriID}")]
        public List<string> TumHesaplar(int musteriId)
        {
            //....
            return new()
            {
                "135792468",
                "019283745",
                "085261060"
            };
        }

        //This endpoint illustrates how to run programatically.
        [HttpGet("{musteriId}")]
        public async Task Sample(int musteriId)
        {
            HttpClient httpClient = new HttpClient();
            DiscoveryDocumentResponse discovery = await httpClient.GetDiscoveryDocumentAsync("http://localhost:5000");
            ClientCredentialsTokenRequest tokenRequest = new ClientCredentialsTokenRequest();
            tokenRequest.ClientId = "HalkBankasi";
            tokenRequest.ClientSecret = "halkbank";
            tokenRequest.Address = discovery.TokenEndpoint;

            TokenResponse tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest);
            httpClient.SetBearerToken(tokenResponse.AccessToken);

            HttpResponseMessage response = await httpClient.GetAsync("http://localhost:7000/api/halkbank/bakiye/3000");

            if (response.IsSuccessStatusCode)
            {
                //Following code gives an error about converting json to int. It is not crucial.
                //var bakiye = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStringAsync());
            }

        }
    }
}
