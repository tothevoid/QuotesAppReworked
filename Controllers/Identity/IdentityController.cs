using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Models.Frontend;
using QuotesExchangeApp.Models.Identity;
using QuotesExchangeApp.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Controllers.Identity
{
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
    [ApiController]
    public class IdentityController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public IdentityController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IOptions<JwtOptions> jwtOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpGet]
        public async Task SignOut() => await _signInManager.SignOutAsync();

        //TODO: return generated auth token
        [HttpPost]
        public async Task<User> SignUp(SignInModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PasswordHash = model.Password };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                //token generation
                //var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl };
                await _signInManager.SignInAsync(user, isPersistent: false);
                return new User() { Name = model.Email }; 
            }

            return null;
        }
 
        [HttpPost]
        public async Task<UserIdentity> GetToken(SignInModel model)
        {
            //var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false);
            var identity = await GetIdentity(model.Email, model.Password);

            if (identity != null)
            {
                var now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                        issuer: _jwtOptions.Issuer,
                        audience: _jwtOptions.Audience,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(_jwtOptions.Lifetime)),
                        signingCredentials: new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Key)),
                                SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                return new UserIdentity { Token = encodedJwt };
            }

            //fix
            return null;
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin")
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}
