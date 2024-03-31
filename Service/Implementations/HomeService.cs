using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Enum;
using CourseTry1.Domain.Response;
using CourseTry1.Domain.ViewModels.File;
using CourseTry1.Domain.ViewModels.Group;
using CourseTry1.Domain.ViewModels.User;
using CourseTry1.Service.Interfaces;
using OfficeOpenXml;

namespace CourseTry1.Service.Implementations
{
    public class HomeService : IHomeService
    {
        private readonly IAccountRepository<User> repository;
        private readonly IFileRepository fileRepository;
        private readonly IExcelFileRepository excelFileRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IWebHostEnvironment appEnvironment;

        public HomeService(IAccountRepository<User> repository,
            IFileRepository fileRepository,
            IWebHostEnvironment appEnvironment,
            IExcelFileRepository excelFileRepository,
            IGroupRepository groupRepository,
            IProfileRepository profileRepository)
        {
            this.repository = repository;
            this.fileRepository = fileRepository;
            this.appEnvironment = appEnvironment;
            this.excelFileRepository = excelFileRepository;
            this.groupRepository = groupRepository;
            this.profileRepository = profileRepository;
        }

        public BaseResponse<IEnumerable<FileViewModel>> GetFiles()
        {
            try
            {
                var filesViewModel = fileRepository.GetAll().Select(x => new FileViewModel()
                {
                    Name = x.Name,
                    IsSelected = x.IsSelected
                });

                return new BaseResponse<IEnumerable<FileViewModel>>()
                {
                    Data = filesViewModel,
                    StatusCode = StatusCode.Ok,
                    Description = "Успешно получили все файлы"
                };
            }
            catch
            {
                return new BaseResponse<IEnumerable<FileViewModel>>()
                {
                    Data = new List<FileViewModel>(),
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

        public async Task<BaseResponse<IEnumerable<FileViewModel>>> DeleteFile(string name)
        {
            var filesViewModel = fileRepository.GetAll().Select(x => new FileViewModel()
            {
                Name = x.Name,
                IsSelected = x.IsSelected
            });

            try
            {
                var excelFile = await fileRepository.GetByName(name);
                await excelFileRepository.Clear();

                if (excelFile != null)
                {
                    await fileRepository.DeleteFile(excelFile);
                    fileRepository.Delete(excelFile.Name, appEnvironment);

                    return new BaseResponse<IEnumerable<FileViewModel>>
                    {
                        Description = "Успешно удалили",
                        StatusCode = Domain.Enum.StatusCode.Ok,
                        Data = filesViewModel
                    };
                }

                return new BaseResponse<IEnumerable<FileViewModel>>
                {
                    Description = "Файл не существует",
                    StatusCode = Domain.Enum.StatusCode.BadFile,
                    Data = filesViewModel
                };
            }
            catch
            {
                return new BaseResponse<IEnumerable<FileViewModel>>
                {
                    Description = "Ошибка при удалении",
                    StatusCode = Domain.Enum.StatusCode.BadRequest,
                    Data = filesViewModel
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<FileViewModel>>> SelectFile(string name)
        {
            var filesViewModel = fileRepository.GetAll().Select(x => new FileViewModel()
            {
                Name = x.Name,
                IsSelected = x.IsSelected
            });

            try
            {
                var excelFile = await fileRepository.GetByName(name);

                if (excelFile != null)
                {
                    if (excelFile.IsSelected)
                    {
                        return new BaseResponse<IEnumerable<FileViewModel>>
                        {
                            Description = "Файл верного формата выбран как текущий",
                            StatusCode = Domain.Enum.StatusCode.Ok,
                            Data = filesViewModel
                        };
                    }

                    //await excelFileRepository.Clear();

                    var isCorrectFile = await ParseExcel(excelFile.Name);

                    if (isCorrectFile)
                    {
                        excelFile.IsSelected = true;

                        await fileRepository.UpdateFile(excelFile);

                        return new BaseResponse<IEnumerable<FileViewModel>>
                        {
                            Description = "Файл верного формата выбран как текущий",
                            StatusCode = Domain.Enum.StatusCode.Ok,
                            Data = filesViewModel
                        };
                    }

                    return new BaseResponse<IEnumerable<FileViewModel>>
                    {
                        Description = "Неверный формат Excel файла",
                        StatusCode = Domain.Enum.StatusCode.BadFile,
                        Data = filesViewModel
                    };
                }

                return new BaseResponse<IEnumerable<FileViewModel>>
                {
                    Description = "Выбор несуществующего файла",
                    StatusCode = Domain.Enum.StatusCode.BadFile,
                    Data = filesViewModel
                };
            }
            catch
            {
                return new BaseResponse<IEnumerable<FileViewModel>>
                {
                    Description = "Ошибка выборка избранного",
                    StatusCode = Domain.Enum.StatusCode.BadRequest,
                    Data = filesViewModel
                };
            }
        }

        public BaseResponse<IEnumerable<UserViewModel>> SortedUser(string qutry)
        {
            if (qutry == "" || string.IsNullOrWhiteSpace(qutry))
            {
                var users = repository.GetAll();

                var usersViewModel = new List<UserViewModel>();

                foreach (var user in users)
                {
                    usersViewModel.Add(new UserViewModel()
                    {
                        Id = user.Id,
                        Login = user.Login,
                        Role = user.Role
                    });
                }

                return new BaseResponse<IEnumerable<UserViewModel>>
                {
                    Data = usersViewModel,
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

                var usersViewModel = new List<UserViewModel>();

                foreach (var user in users)
                {
                    usersViewModel.Add(new UserViewModel()
                    {
                        Id = user.Id,
                        Login = user.Login,
                        Role = user.Role
                    });
                }

                return new BaseResponse<IEnumerable<UserViewModel>>
                {
                    Data = usersViewModel,
                    Description = "Успешный поиск",
                    StatusCode = StatusCode.Ok
                };
            }
            catch
            {
                return new BaseResponse<IEnumerable<UserViewModel>>()
                {
                    Description = "ошибка при парсинге строки",
                    Data = null,
                    StatusCode = Domain.Enum.StatusCode.BadRequest
                };
            }
        }

        public async Task<BaseResponse<User>> UpdateUser(int id, Role role)
        {
            try
            {
                var user = await repository.GetById(id);

                if (user.Role != role)
                {
                    user.Role = role;

                    await repository.Update(user);

                    return new BaseResponse<User>()
                    {
                        Data = user,
                        StatusCode = StatusCode.Ok,
                        Description = "Успешно обновили"
                    };
                }

                return new BaseResponse<User>
                {
                    Description = "Пользователь не был изменен",
                    StatusCode = StatusCode.Ok,
                    Data = user
                };
            }
            catch
            {
                return new BaseResponse<User>
                {
                    Description = "Ошибка при обновлении",
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

        public BaseResponse<IEnumerable<GroupViewModel>> GetGroups()
        {
            try
            {
                var groups = groupRepository.GetGroups().Select(x => new GroupViewModel()
                {
                    Id = x.Id,
                    Name = x.NameGroup
                });
                return new BaseResponse<IEnumerable<GroupViewModel>>
                {
                    Description = "Успешно получили группы",
                    Data = groups,
                    StatusCode = StatusCode.Ok
                };
            }
            catch
            {
                return new BaseResponse<IEnumerable<GroupViewModel>>()
                {

                    Description = "Ошибка во время выполнения",
                    StatusCode = StatusCode.BadRequest,
                    Data = new List<GroupViewModel>()
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<GroupViewModel>>> GetSelectedGroup(string name)
        {
            try
            {
                var user = await repository.GetByLogin(name);

                var groups = (await profileRepository.GetSelectedGroup(user)).Select(x => new GroupViewModel()
                {
                    Id = x.Id,
                    Name = x.NameGroup
                });

                return new BaseResponse<IEnumerable<GroupViewModel>>()
                {

                    Description = "Успешно получили выбранные группы",
                    StatusCode = StatusCode.Ok,
                    Data = groups
                };
            }
            catch
            {
                return new BaseResponse<IEnumerable<GroupViewModel>>()
                {

                    Description = "Ошибка во время выполнения",
                    StatusCode = StatusCode.BadRequest,
                    Data = new List<GroupViewModel>()
                };
            }
        }

        public async Task<BaseResponse<Profile>> DeleteGroupToUser(string name, int idGroup)
        {
            try
            {
                var user = await repository.GetByLogin(name);
                var group = await groupRepository.GetGroupById(idGroup);

                if (user == null || group == null)
                {
                    return new BaseResponse<Profile>()
                    {

                        Description = "Не существует такого пользователя или группы",
                        StatusCode = StatusCode.BadRequest,
                        Data = new Profile()
                    };
                }

                var profile = profileRepository.DeleteGroup(user, group);

                return new BaseResponse<Profile>()
                {
                    Description = "Успешно удалили пользователя",
                    StatusCode = StatusCode.Ok,
                    Data = await profile
                };
            }
            catch
            {
                return new BaseResponse<Profile>()
                {

                    Description = "Ошибка во время выполнения",
                    StatusCode = StatusCode.BadRequest,
                    Data = new Profile()
                };
            }
        }

        public async Task<BaseResponse<Profile>> AddGroupToUser(string name, int idGroup)
        {
            try
            {
                var user = await repository.GetByLogin(name);
                var group = await groupRepository.GetGroupById(idGroup);

                if (user == null || group == null)
                {
                    return new BaseResponse<Profile>()
                    {

                        Description = "Не существует такого пользователя или группы",
                        StatusCode = StatusCode.BadRequest,
                        Data = new Profile()
                    };
                }

                await profileRepository.AddProfileUser(user);

                var profile = await profileRepository.AddGroup(user, group);

                return new BaseResponse<Profile>()
                {
                    Description = "Успешно добавили группу",
                    StatusCode = StatusCode.Ok,
                    Data = profile
                };
            }
            catch
            {
                return new BaseResponse<Profile>()
                {

                    Description = "Ошибка во время выполнения",
                    StatusCode = StatusCode.BadRequest,
                    Data = new Profile()
                };
            }
        }

        public async Task<BaseResponse<DayWeek>> GetGroup(int idGroup, DayOfWeek dayOfWeek)
        {
            try
            {
                return null;
            }
            catch
            {
                return new BaseResponse<DayWeek>()
                {
                    Description = "Ошибка при получении группы",
                    StatusCode = StatusCode.BadRequest,
                    Data = new DayWeek()
                };
            }
        }
    }
}
