using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Enum;
using CourseTry1.Domain.Response;
using CourseTry1.Domain.ViewModels.Group;
using CourseTry1.Models;
using CourseTry1.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OfficeOpenXml;

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
        // TODO #11 переписать некоторые запросы на ajax
        // TODO #12 если смогу добавить кнопку добавить файл с сайта бнту
        // TODO #13 при добавлении группы отображать без года
        // TODO #8 Переписать кэширование на redis
        // TODO #9 Перенести бд в Docker
        // TODO #10 В конце можно поместить все приложение в контейнер Docker
        // TODO #5 сделать разбивку на недели (в последнию очередь а то обратно будет ебка)
        // TODO #6 ПИДАРАСЫ НЕ МОГУТ ЗАПОЛНИТЬ НОРМАЛЬНО EXCEL ТАБЛИЦУ И ИНОГДА НЕТУ РАЗДЕЛИТЕЛЯ ИЛИ ПРЕПОДА Т Е ЧТО ПРАВИЛЬНО РАЗРАБ 
        public async Task<IActionResult> Index(int cource = 1)
        {
            var responseGetGroups = homeService.GetGroups(cource);

            BaseResponse<IEnumerable<GroupViewModel>> responseGetSelectedGroups = null;
            //BaseResponse<IEnumerable<int>> responseGetCources = null;

            if (User.Identity!.IsAuthenticated)
            {
                responseGetSelectedGroups = await homeService.GetSelectedGroup(User.Identity!.Name);
                //responseGetCources = await homeService.GetCources();
            }

            return View(new IndexViewModel()
            {
                Groups = responseGetGroups.Data.ToList() ?? new List<GroupViewModel>(),
                TrackGroups = responseGetSelectedGroups == null ?
                new List<GroupViewModel>() : responseGetSelectedGroups.Data.ToList(),
                /*Cources = responseGetCources == null ?
                new List<int>() : responseGetCources.Data.ToList()*/
            });


        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteGroup(int idGroup)
        {
            var response = await homeService.DeleteGroupToUser(User.Identity.Name, idGroup);

            if(response.StatusCode != Domain.Enum.StatusCode.Ok)
            {
                ModelState.AddModelError("", response.Description);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddGroup(int idGroup)
        {

            var response = await homeService.AddGroupToUser(User.Identity!.Name, idGroup);

            if(response.StatusCode == Domain.Enum.StatusCode.Ok)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult SettingRole()
        {
            var response = homeService.SortedUser("");

            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
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

            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
            {
                return View(response.Data);
            }

            ModelState.AddModelError("", response.Description);

            return RedirectToAction("SettingRole", "Home", query);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Methodist")]
        public async Task<IActionResult> AddFiles(IFormFile file)
        {
            var response = await homeService.AddFile(file);

            if (response.StatusCode != Domain.Enum.StatusCode.Ok)
            {
                ModelState.AddModelError("", response.Description);
            }

            return RedirectToAction("SettingFiles");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Methodist")]
        public IActionResult SettingFiles()
        {
            var response = homeService.GetFiles();

            if(response.StatusCode != Domain.Enum.StatusCode.Ok)
            { 
                ModelState.AddModelError("", response.Description);
            }

            return View(response.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Methodist")]
        public async Task<IActionResult> DeleteFile(string name)
        {
            var response = await homeService.DeleteFile(name);

            if(response.StatusCode != Domain.Enum.StatusCode.Ok)
            {
                ModelState.AddModelError("", response.Description);
            }

            return View("SettingFiles", response.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Methodist")]
        public async Task<IActionResult> SelectFile(string name)
        {
            var response = await homeService.SelectFile(name);

            if(response.StatusCode != Domain.Enum.StatusCode.Ok)
            {
                ModelState.AddModelError("", response.Description);
            }

            return View("SettingFiles", response.Data);
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

        [HttpGet]
        public IActionResult GetParseShadule()
        {
            var excelFileDirectory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files";

            var excelFilePath = Path.Combine(excelFileDirectory, "Расписание ФИТР.xlsx");

            FileInfo existFile = new FileInfo(excelFilePath);
            using (ExcelPackage package = new ExcelPackage(existFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int colCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                List<SheduleGroup> groups = new();

                for (int i = 3; i <= colCount; i += 6)
                {
                    if (worksheet.Cells[2, i].Value != null)
                    {
                        groups.Add(new SheduleGroup()
                        {
                            NameGroup = worksheet.Cells[2, i].Value.ToString()!
                        });

                        groups[^1].Weeks = new List<DayWeek>();

                        // Заполняем дни недели
                        int scoreDay = 1;
                        for (int j = 3; j <= rowCount; j+=2)
                        {
                            if (worksheet.Cells[j, 1].Value != null)
                            {
                                // Добавли название дней
                                groups[^1].Weeks.Add(new DayWeek()
                                {
                                    DayOfWeek = (DayOfWeek)(scoreDay),
                                    PairingTime = new()
                                });
                                scoreDay++;

                                // проходимя по строкам и добавляем время - предмет
                                for (int k = j; k <= rowCount; k++)
                                {
                                    if (worksheet.Cells[k, 1].Value != null && k != j)
                                    {
                                        break;
                                    }
                                    if (worksheet.Cells[k, i].Value != null)
                                    {
                                        if (worksheet.Cells[k, 2].Value == null)
                                        {
                                            groups[^1].Weeks[^1].PairingTime
                                            .Add(new Subject()
                                            {
                                                Time = groups[^1].Weeks[^1].PairingTime[^1].Time,
                                                Name = worksheet.Cells[k, i].Value.ToString()!
                                            });
                                        }
                                        else
                                        {
                                            groups[^1].Weeks[^1].PairingTime
                                            .Add(new Subject()
                                            {
                                                Time = worksheet.Cells[k, 2].Value.ToString()!,
                                                Name = worksheet.Cells[k, i].Value.ToString()!
                                            });
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var group in groups)
                {
                    Console.WriteLine(group.NameGroup);
                    foreach (var i in group.Weeks)
                    {
                        Console.WriteLine($"\t{i.DayOfWeek.ToString()}");
                        foreach (var j in i.PairingTime)
                        {
                            Console.WriteLine($"\t\t{j.Time}:{j.Name}");
                        }
                    }
                }
            }

            return View("Setting");
        }
    }
}
