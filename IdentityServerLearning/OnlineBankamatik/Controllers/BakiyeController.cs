using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBankamatik.Controllers
{
    public class BakiyeController : ControllerBase
    {
        [Authorize(Roles = "admin")]
        public IActionResult BakiyeGor()
        {
            return Ok(1000);
        }
        [Authorize(Roles = "moderator")]
        public IActionResult HareketDokumu()
        {
            return Ok("Hareket dökümü");
        }
        [Authorize(Roles = "admin, moderator")]
        public IActionResult CiktiAl()
        {
            return Ok("Döküman alınıyor");
        }
    }
}
