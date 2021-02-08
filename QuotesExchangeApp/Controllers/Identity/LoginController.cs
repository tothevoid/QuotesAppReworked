using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Models.Identity;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Controllers.Identity
{
    [Route("api/identity/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class LoginController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<bool> OnPostAsync(SignInModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            return result.Succeeded;
        }
    }
}


