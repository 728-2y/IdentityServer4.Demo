using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.IdentityServer.Models;
using WebApi.IdentityServer.Models.Account;

namespace WebApi.IdentityServer.Controllers
{
    [Route("[controller]")]
    public class AccountController: ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("signIn")]
        public IActionResult SignIn(string returnUrl= null)
        {

            return Ok();
        }

        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody]SignUpModel signUpUser,string returnUrl=null)
        {
            try
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return BadRequest("returnUrl is not null or empty");
                }

                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(signUpUser.Email);
                    if (user == null)
                    {
                        var account = new ApplicationUser()
                        {
                            UserName = signUpUser.UserName,
                            Email = signUpUser.Email,
                            CountryCode = signUpUser.CountryCode,
                            Address = signUpUser.Address
                        };
                        var result = await _userManager.CreateAsync(account, signUpUser.Password);
                        if (result.Succeeded)
                        {
                            var claims = new Claim[] {
                            new Claim(JwtClaimTypes.Email,signUpUser.Email),
                            new Claim(JwtClaimTypes.Subject,signUpUser.UserName)
                            };
                            await _userManager.AddClaimsAsync(account, claims);

                             await _signInManager.SignInAsync(account, isPersistent: false);

                            return Redirect(returnUrl);
                        }

                        return BadRequest(result.Errors);
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest($"Errors:{ex.Message}");
            }            
        }
    }
}
