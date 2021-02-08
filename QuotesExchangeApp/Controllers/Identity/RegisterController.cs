using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Models.Identity;
using System;
using System.Text;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Controllers.Identity
{
    [AllowAnonymous]
    public class RegisterController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public RegisterController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }


        //TODO: return generated auth token
        [HttpPost]
        public async Task OnPostAsync(SignInModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
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


