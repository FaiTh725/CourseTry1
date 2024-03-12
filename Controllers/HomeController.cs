using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Enum;
using CourseTry1.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CourseTry1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult SettingRole()
        {
            var response = homeService.SortedUser("");

            if(response.StatusCode == Domain.Enum.StatusCode.Ok)
            {
                return View(response.Data);
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult SettingRole(string query)
        {
            var response = homeService.SortedUser(query);

            if(response.StatusCode == Domain.Enum.StatusCode.Ok)
            {
                return View(response.Data);
            }

            ModelState.AddModelError("", response.Description);

            return RedirectToAction("SettingRole", "Home", query);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, Role role)
        {
            var response = await homeService.UpdateUser(id, role);

            if (response.StatusCode != Domain.Enum.StatusCode.Ok)
            {
                ModelState.AddModelError("", response.Description);
            }

            return View("SettingRole", homeService.SortedUser("").Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Methodist")]
        public async Task<IActionResult> AddFiles(IFormFile file)
        {
            var response = await homeService.AddFile(file);

            if (response.StatusCode != Domain.Enum.StatusCode.Ok)
            {
                ModelState.AddModelError("", response.Description);
            }

            return View("Index");
        }
    }
}
