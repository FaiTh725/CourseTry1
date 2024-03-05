using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Response;
using CourseTry1.Domain.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace CourseTry1.Service.Interfaces
{
    public interface IAccountService
    {
        public Task<BaseResponse<LoginViewModel>> Register(RegisterViewModel entity);
        public Task<BaseResponse<LoginViewModel>> Login(LoginViewModel entity);
        public Task<BaseResponse<IEnumerable<User>>> GetUsers();
        public Task<BaseResponse<User>> GetUserById(int id);
        public Task<BaseResponse<User>> GetUserByLogin(string login);
    }
}
