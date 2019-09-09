using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.IdentityServer.Models;

namespace WebApi.IdentityServer.Contexts
{
    public class IdentityContext:IdentityDbContext<ApplicationUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> context) : base(context)
        {
        }

        protected override void OnModelCreating(ModelBuilder buidler)
        {
            base.OnModelCreating(buidler);
        }
    }
}
