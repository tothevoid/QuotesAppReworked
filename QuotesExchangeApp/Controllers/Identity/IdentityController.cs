using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Controllers.Identity
{
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
    [ApiController]
    public class IdentityController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManage;

        public IdentityController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManage = userManager;
        }

        [HttpPost]
        public async Task<bool> SignIn(SignInModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false);
            return result.Succeeded;
        }

        [HttpGet]
        public async Task SignOut() => await _signInManager.SignOutAsync();

        //TODO: return generated auth token
        [HttpPost]
        public async Task SignUp(SignInModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PasswordHash = model.Password };
            var result = await _userManage.CreateAsync(user);
            if (result.Succeeded)
            {
                //token generation
                //var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl };
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
        }

        private async Task<bool> SendConfirmationEmailAsync() => throw new NotImplementedException();
        //_userManager.Options.SignIn.RequireConfirmedAccount
        //TODO: email integration
        //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
        //       $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
    }
}
