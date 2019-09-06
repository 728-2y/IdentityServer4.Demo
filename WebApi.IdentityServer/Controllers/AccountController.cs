using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.IdentityServer.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class AccountController: ControllerBase
    {
        [HttpGet("signIn")]
        public IActionResult SignIn(string returnUrl= null)
        {

            return Ok();
        }
    }
}
