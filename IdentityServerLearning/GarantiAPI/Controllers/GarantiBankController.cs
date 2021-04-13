using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GarantiAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class GarantiBankController : ControllerBase
    {
        [HttpGet("{musteriId}")]
        [Authorize(Policy = "ReadGaranti")]
        public double Bakiye(int musteriId)
        {
            return 1000;
        }
        [HttpGet("{musteriId}")]
        public List<string> TumHesaplar(int musteriId)
        {
            return new()
            {
                "123456789",
                "987654321",
                "564738291"
            };
        }

        //If we don't have scope AllGaranti, returning http status code will be 403(forbidden), not 401
        [HttpGet("{musteriId}/{tutar}")]
        [Authorize(Policy = "AllGaranti")]
        public double YatirimYap(int musteriId, double tutar)
        {
            return tutar * 0.5;
        }
    }
}
