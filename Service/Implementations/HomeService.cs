using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Enum;
using CourseTry1.Domain.Response;
using CourseTry1.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace CourseTry1.Service.Implementations
{
    public class HomeService : IHomeService
    {
        private readonly IAccountRepository<User> repository;

        public HomeService(IAccountRepository<User> repository)
        {
            this.repository = repository;
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

                if(!string.IsNullOrEmpty(role))
                {
                    users = users.Where(x => x.Role.ToString() == role);    
                }
                if(!string.IsNullOrEmpty(id))
                {
                    users = users.Where(x => x.Id.ToString() == id);
                }
                else if(!string.IsNullOrEmpty(login))
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

        public async Task<BaseResponse<User>> UpdateUser(int id, Role role)
        {
            try
            {
                var user = await repository.GetById(id);

                if(user.Role != role)
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
    }
}
