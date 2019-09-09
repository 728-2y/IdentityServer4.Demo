﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.IdentityServer.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string CountryCode { get; set; }
        public string  Address { get; set; }
    }
}
