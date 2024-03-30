using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Enum;
using CourseTry1.Domain.Response;
using CourseTry1.Domain.ViewModels.File;
using CourseTry1.Domain.ViewModels.Group;
using CourseTry1.Domain.ViewModels.User;

namespace CourseTry1.Service.Interfaces
{
    public interface IHomeService
    {
        public BaseResponse<IEnumerable<UserViewModel>> SortedUser(string query);

        public Task<BaseResponse<ExcelFile>> AddFile(IFormFile uploadFile);

        public BaseResponse<IEnumerable<FileViewModel>> GetFiles();

        public Task<BaseResponse<IEnumerable<FileViewModel>>> DeleteFile(string name);

        public Task<BaseResponse<IEnumerable<FileViewModel>>> SelectFile(string name);

        public Task<BaseResponse<User>> UpdateUser(int id, Role role);

        public BaseResponse<IEnumerable<GroupViewModel>> GetGroups();

        public Task<BaseResponse<IEnumerable<GroupViewModel>>> GetSelectedGroup(string name);

        public Task<BaseResponse<Profile>> AddGroupToUser(string name, int idGroup);

    }
}
