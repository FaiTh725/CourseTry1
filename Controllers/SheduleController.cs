using CourseTry1.Domain.Enum;
using CourseTry1.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTry1.Controllers
{
    public class SheduleController : Controller
    {
        private readonly IGroupService groupService;

        public SheduleController(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetShedule(int idGroup, DayOfWeek? dayOfWeek, int cource = 1, Week weeks = Week.first)
        {
            var response = await groupService.GetDayShedule(idGroup, dayOfWeek, cource, weeks);

            if(response.StatusCode != Domain.Enum.StatusCode.Ok)
            {
                ModelState.AddModelError("", response.Description);
            }

            return View("SheduleDay", response.Data);  
        }
    }
}
