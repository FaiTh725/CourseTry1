using CourseTry1.Domain.Response;
using CourseTry1.Domain.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace CourseTry1.Service.Interfaces
{
    public interface IAccountService
    {
        public Task<BaseResponse<LoginViewModel>> Register(RegisterViewModel entity);
        public Task<BaseResponse<LoginViewModel>> Login(LoginViewModel entity);

        //public Task<BaseResponse<LoginViewModel>> GetByData(LoginViewModel entity);
    }
}
