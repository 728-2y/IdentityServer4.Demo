using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class AccountController: ApiControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromBody]SignInModel signIn,string returnUrl= null)
        {
            try
            {
                if (string.IsNullOrEmpty(returnUrl))
                    return BadRequest("returnUrl is null or empty");

                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(signIn.Email, signIn.Password, signIn.IsRememberMe, true);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl);
                    }

                    return BadRequest("Authentication failed");
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
                return InternalServerError($"Errors:{ex.Message}");
            }
        }

        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody]SignUpModel signUpUser,string returnUrl=null)
        {
            try
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return BadRequest("returnUrl is null or empty");
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

                            // await _signInManager.SignInAsync(account, isPersistent: false);

                            return Redirect(returnUrl);
                        }

                        return BadRequest(result.Errors);
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
                return InternalServerError($"Errors:{ex.Message}");
            }            
        }
    }
}
