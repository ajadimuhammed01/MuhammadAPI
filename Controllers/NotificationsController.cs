using System;
using Microsoft.AspNetCore.Mvc;

namespace  techHowdy.API.Controllers
{
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {

        [HttpGet("[action]")]
        public IActionResult EmailConfirmed(string userId, string code)
        {
            return View();
        }
    }
}