using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Enum;
using CourseTry1.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

//ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

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
                            NameGroup = $"{worksheet.Cells[2, i].Value} - {i}"
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
                                            .Add(new KeyValuePair<string, string>
                                            (groups[^1].Weeks[^1].PairingTime[^1].Key, worksheet.Cells[k, i].Value.ToString()));
                                        }
                                        else
                                        {
                                            groups[^1].Weeks[^1].PairingTime
                                            .Add(new KeyValuePair<string, string>
                                            (worksheet.Cells[k, 2].Value.ToString(), worksheet.Cells[k, i].Value.ToString()));
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
                            Console.WriteLine($"\t\t{j.Key}:{j.Value}");
                        }
                    }
                }
                /*for (int row = 1; row <= rowCount; row++)
                {
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (worksheet.Cells[row, col].Value != null)
                        {
                            Console.Write($"{worksheet.Cells[row, col].Value} - {row}:{col}; ");
                        }
                    }
                    Console.WriteLine();
                }*/
            }

            return View("Setting");
        }
    }
}
