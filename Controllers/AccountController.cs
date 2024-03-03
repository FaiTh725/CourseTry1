using CourseTry1.Domain.Enum;
using CourseTry1.Domain.ViewModels.Account;
using CourseTry1.Service;
using CourseTry1.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace CourseTry1.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IConfigurationSection jwtSetting;

        public AccountController(IAccountService accountService, IConfiguration setting)
        {
            this.accountService = accountService;
            jwtSetting = setting.GetSection("JWTConfiguration");
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModel model = new();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await accountService.Login(model);

                if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                {
                    var identity = GetIdentity(response.Data);

                    var jwt = new JwtSecurityToken(
                        issuer: jwtSetting["Issuer"],
                        audience: jwtSetting["Audience"],
                        claims: identity.Claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
                        signingCredentials: new SigningCredentials(Config.GetSymmetricSecurityKey(jwtSetting["Key"]), 
                        SecurityAlgorithms.HmacSha256)
                        );

                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                    HttpContext.Session.SetString("JWToken", encodedJwt);


                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Login", response.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Registered()
        {
            RegisterViewModel model = new();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Registered(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await accountService.Register(model);

                if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                {
                    return RedirectToAction("");
                }
                else
                {
                    ModelState.AddModelError("Login", response.Description);
                }

            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index");
        }

        private ClaimsIdentity GetIdentity(LoginViewModel model)
        {
            if(model != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, model.Login),
                    // все кто зарегался по умолчанию пользователи
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, Role.User.ToString())
                };

                var claimIdentity = new ClaimsIdentity(claims, "Token",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                return claimIdentity;
                //return claims;
            }

            return null;
        }
    }
}
