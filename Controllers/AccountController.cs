using CourseTry1.Domain.ViewModels.Account;
using CourseTry1.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTry1.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
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
                    return RedirectToAction();
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
    }
}
