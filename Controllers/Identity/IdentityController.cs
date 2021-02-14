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

        [HttpPost]
        public async Task<string> SignUp(SignInModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PasswordHash = model.Password };
            var result = await _userManager.CreateAsync(user);
            return (result.Succeeded) ?
                GenerateToken(user):
                string.Empty;
        }

        [HttpPost]
        public async Task<string> GetToken(SignInModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            return GenerateToken(user);
        }

        private string GenerateToken(ApplicationUser user)
        {
            var identity = GetIdentity(user);

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
                return encodedJwt;
            }

            return string.Empty;
        }

        private ClaimsIdentity GetIdentity(ApplicationUser user)
        {
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
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
