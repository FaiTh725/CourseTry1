﻿using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Enum;
using CourseTry1.Domain.Response;
using CourseTry1.Service.Interfaces;
using OfficeOpenXml;

namespace CourseTry1.Service.Implementations
{
    public class HomeService : IHomeService
    {
        private readonly IAccountRepository<User> repository;
        private readonly IFileRepository fileRepository;
        private readonly IExcelFileRepository excelFileRepository;
        private readonly IWebHostEnvironment appEnvironment;

        public HomeService(IAccountRepository<User> repository,
            IFileRepository fileRepository,
            IWebHostEnvironment appEnvironment,
            IExcelFileRepository excelFileRepository)
        {
            this.repository = repository;
            this.fileRepository = fileRepository;
            this.appEnvironment = appEnvironment;
            this.excelFileRepository = excelFileRepository;
        }

        public BaseResponse<IEnumerable<ExcelFile>> GetFiles()
        {
            try
            {
                var files = fileRepository.GetAll().ToList();

                if (files == null)
                {
                    files = new List<ExcelFile>();
                }

                return new BaseResponse<IEnumerable<ExcelFile>>()
                {
                    Data = files,
                    StatusCode = StatusCode.Ok,
                    Description = "Успешно получили все файлы"
                };
            }
            catch
            {
                return new BaseResponse<IEnumerable<ExcelFile>>()
                {
                    Data = null,
                    StatusCode = StatusCode.BadRequest,
                    Description = "Ошибка при получении всех файлов"
                };
            }
        }

        public async Task<BaseResponse<ExcelFile>> AddFile(IFormFile uploadFile)
        {
            try
            {
                if (uploadFile != null)
                {
                    if (await fileRepository.GetByName(uploadFile.FileName) != null)
                    {
                        return new BaseResponse<ExcelFile>
                        {
                            Data = null,
                            StatusCode = StatusCode.FileExcist,
                            Description = "Файл c таким именем существует"
                        };
                    }

                    await fileRepository.Add(uploadFile, appEnvironment);

                    return new BaseResponse<ExcelFile>
                    {
                        Data = null,
                        StatusCode = StatusCode.Ok,
                        Description = "Файл успешно добавлен"
                    };
                }

                return new BaseResponse<ExcelFile>
                {
                    StatusCode = StatusCode.BadFile,
                    Data = null,
                    Description = "Не выбран файл"
                };
            }
            catch
            {
                return new BaseResponse<ExcelFile>()
                {
                    Description = "Ошибка при загрузке файла",
                    Data = null,
                    StatusCode = Domain.Enum.StatusCode.BadRequest
                };
            }
        }

        public BaseResponse<IEnumerable<User>> SortedUser(string qutry)
        {
            if (qutry == "" || string.IsNullOrWhiteSpace(qutry))
            {
                return new BaseResponse<IEnumerable<User>>
                {
                    Data = repository.GetAll(),
                    Description = "Вывод все записей",
                    StatusCode = StatusCode.Ok
                };
            }

            var splitValues = qutry.Split('-');

            splitValues = splitValues.Concat(Enumerable.Repeat("", 3 - splitValues.Length)).ToArray();

            string id = splitValues[0];
            string login = splitValues[1];
            string role = splitValues[2];


            try
            {
                IEnumerable<User> users = repository.GetAll();

                if (!string.IsNullOrEmpty(role))
                {
                    users = users.Where(x => x.Role.ToString() == role);
                }
                if (!string.IsNullOrEmpty(id))
                {
                    users = users.Where(x => x.Id.ToString() == id);
                }
                else if (!string.IsNullOrEmpty(login))
                {
                    users = users.Where(x => x.Login == login);
                }

                return new BaseResponse<IEnumerable<User>>
                {
                    Data = users,
                    Description = "Успешный поиск",
                    StatusCode = StatusCode.Ok
                };
            }
            catch
            {
                return new BaseResponse<IEnumerable<User>>()
                {
                    Description = "ошибка при парсинге строки",
                    Data = null,
                    StatusCode = Domain.Enum.StatusCode.BadRequest
                };
            }
        }


        public async Task<BaseResponse<ExcelFile>> DeleteFile(int id)
        {
            try
            {
                var excelFile = await fileRepository.GetById(id);

                if (excelFile != null)
                {
                    await fileRepository.DeleteFile(excelFile);

                    return new BaseResponse<ExcelFile>
                    {
                        Description = "Успешно удалили",
                        StatusCode = Domain.Enum.StatusCode.Ok,
                        Data = excelFile
                    };
                }

                return new BaseResponse<ExcelFile>
                {
                    Description = "Файл не существует",
                    StatusCode = Domain.Enum.StatusCode.BadFile,
                    Data = excelFile
                };
            }
            catch
            {
                return new BaseResponse<ExcelFile>
                {
                    Description = "Ошибка при удалении",
                    StatusCode = Domain.Enum.StatusCode.BadRequest,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ExcelFile>> SelectFile(int id)
        {
            try
            {
                var excelFile = await fileRepository.GetById(id);

                if (excelFile != null)
                {
                    if(excelFile.IsSelected)
                    {
                        return new BaseResponse<ExcelFile>
                        {
                            Description = "Файл верного формата выбран как текущий",
                            StatusCode = Domain.Enum.StatusCode.Ok,
                            Data = excelFile
                        };
                    }

                    //await excelFileRepository.Clear();

                    var isCorrectFile = await ParseExcel(excelFile.Name);

                    if (isCorrectFile)
                    {
                        excelFile.IsSelected = true;

                        await fileRepository.UpdateFile(excelFile);

                        return new BaseResponse<ExcelFile>
                        {
                            Description = "Файл верного формата выбран как текущий",
                            StatusCode = Domain.Enum.StatusCode.Ok,
                            Data = excelFile
                        };
                    }

                    return new BaseResponse<ExcelFile>
                    {
                        Description = "Неверный формат Excel файла",
                        StatusCode = Domain.Enum.StatusCode.BadFile,
                        Data = null
                    };
                }

                return new BaseResponse<ExcelFile>
                {
                    Description = "Удаление несуществующего файла",
                    StatusCode = Domain.Enum.StatusCode.BadFile,
                    Data = null
                };
            }
            catch
            {
                return new BaseResponse<ExcelFile>
                {
                    Description = "Ошибка изменении",
                    StatusCode = Domain.Enum.StatusCode.BadRequest,
                    Data = null
                };
            }
        }

        private async Task<bool> ParseExcel(string fileName)
        {
            var excelFileDirectory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files";

            var excelFilePath = Path.Combine(excelFileDirectory, fileName);

            FileInfo existFile = new FileInfo(excelFilePath);
            using (ExcelPackage package = new ExcelPackage(existFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int colCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                List<SheduleGroup> groups = new();

                try
                {
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
                            for (int j = 3; j <= rowCount; j += 2)
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

                    await excelFileRepository.Clear();

                    await excelFileRepository.Save(groups);

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
