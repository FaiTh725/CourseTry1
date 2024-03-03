using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Response;
using CourseTry1.Domain.ViewModels.Account;
using CourseTry1.Service.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseTry1.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository<User> repository;
        public AccountService(IAccountRepository<User> repository)
        {
            this.repository = repository;
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
