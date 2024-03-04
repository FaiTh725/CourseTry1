using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Response;
using CourseTry1.Domain.ViewModels.Account;
using CourseTry1.Service.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace CourseTry1.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository<User> repository;
        public AccountService(IAccountRepository<User> repository)
        {
            this.repository = repository;
        }

        public async Task<BaseResponse<User>> GetUserById(int id)
        {
            try
            {
                var user = await repository.GetById(id);

                if(user != null)
                {
                    return new BaseResponse<User> 
                    {
                        Data = user,
                        Description = "Успешно получили пользователя",
                        StatusCode = Domain.Enum.StatusCode.Ok
                    };
                }

                return new BaseResponse<User> 
                { 
                    Data = null,
                    StatusCode = Domain.Enum.StatusCode.UnRegistered,
                    Description = "Данный пользователь не найден"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>
                {
                    Data = null,
                    StatusCode = Domain.Enum.StatusCode.Ok,
                    Description = "Ошибка при получении пользователей"
                };
            }
        }

        public async Task<BaseResponse<User>> GetUserByLogin(string login)
        {
            try
            {
                var user = await repository.GetByLogin(login);

                if(user != null)
                {
                    return new BaseResponse<User>
                    {
                        Data = user,
                        Description = "Успешно получили пользователя",
                        StatusCode = Domain.Enum.StatusCode.Ok
                    };
                }

                return new BaseResponse<User>
                {
                    Data = null,
                    StatusCode = Domain.Enum.StatusCode.UnRegistered,
                    Description = "Данный пользователь не найден"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>
                {
                    Data = null,
                    StatusCode = Domain.Enum.StatusCode.Ok,
                    Description = "Ошибка при получении пользователей"
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = repository.GetAll();
            
                return new BaseResponse<IEnumerable<User>> 
                { 
                    Data = users,
                    StatusCode = Domain.Enum.StatusCode.Ok,
                    Description = "Успешно получили пользователей"
                };
            }
            catch (Exception ex) 
            {
                return new BaseResponse<IEnumerable<User>>
                {
                    Data = null,
                    StatusCode = Domain.Enum.StatusCode.BadRequest,
                    Description = "Ошибка при получении пользователей"
                };
            }

        }

        public async Task<BaseResponse<LoginViewModel>> Login(LoginViewModel entity)
        {
            try
            {
                var user = await repository.GetAll().FirstOrDefaultAsync(x => x.Login == entity.Login && x.Password == entity.Password);

                if(user == null)
                {
                    return new BaseResponse<LoginViewModel>
                    {
                        Description = "Неверный логин или пароль",
                        Data = null,
                        StatusCode = Domain.Enum.StatusCode.UnRegistered
                    };
                }

                return new BaseResponse<LoginViewModel>
                {
                    Description = "Успешный вход",
                    Data = entity,
                    StatusCode = Domain.Enum.StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<LoginViewModel>
                {
                    Description = ex.Message,
                    Data = null,
                    StatusCode = Domain.Enum.StatusCode.BadRequest
                };
            }

        }

        public async Task<BaseResponse<LoginViewModel>> Register(RegisterViewModel entity)
        {
            try
            {
                var user = await repository.GetAll().FirstOrDefaultAsync(x => x.Login == entity.Login);

                if(user == null)
                {
                    if(!entity.Password.Any(char.IsDigit))
                    {
                        return new BaseResponse<LoginViewModel>()
                        {
                            Description = "Пароль должен содержать цифру",
                            Data = null,
                            StatusCode = Domain.Enum.StatusCode.IncorrectPassword
                        };
                    }
                    else if(!entity.Password.Any(char.IsLetter))
                    {
                        return new BaseResponse<LoginViewModel>()
                        {
                            Description = "Пароль должен содержать букву",
                            Data = null,
                            StatusCode = Domain.Enum.StatusCode.IncorrectPassword
                        };
                    }

                    await repository.Add(new User() {
                        Login = entity.Login,
                        Password = entity.Password,
                    });

                    return new BaseResponse<LoginViewModel>()
                    {
                        Description = "Успешно зарегистрирован",
                        Data = null,
                        StatusCode = Domain.Enum.StatusCode.Ok
                    };
                }

                return new BaseResponse<LoginViewModel> 
                { 
                    Description = "Уже зарегистрирован",
                    Data = null,
                    StatusCode = Domain.Enum.StatusCode.RegisteredUser
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<LoginViewModel>
                {
                    Description = ex.Message,
                    Data = null,
                    StatusCode = Domain.Enum.StatusCode.BadRequest
                };
            }
        }
    }
}
