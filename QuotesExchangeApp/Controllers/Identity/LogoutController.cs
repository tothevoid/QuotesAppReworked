using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuotesExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Controllers.Identity
{
    public class LogoutController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        public LogoutController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task OnGet() => await _signInManager.SignOutAsync();
        
    }
}
