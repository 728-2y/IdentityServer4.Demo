using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.IdentityServer.Models.Account
{
    public class SignUpModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }

        public string Address { get; set; }
    }
}
