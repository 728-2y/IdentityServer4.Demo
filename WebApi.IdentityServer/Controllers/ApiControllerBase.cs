using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.IdentityServer.Exceptions;

namespace WebApi.IdentityServer.Controllers
{
    public class ApiControllerBase:ControllerBase
    {
        protected InternalServerErrorObjectResult InternalServerError(object value)
        {
            return new InternalServerErrorObjectResult(value);
        }
    }
}
