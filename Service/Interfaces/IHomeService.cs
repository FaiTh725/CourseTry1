using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Enum;
using CourseTry1.Domain.Response;

namespace CourseTry1.Service.Interfaces
{
    public interface IHomeService
    {
        public BaseResponse<IEnumerable<User>> SortedUser(string query);

        public Task<BaseResponse<ExcelFile>> AddFile(IFormFile uploadFile);

        public BaseResponse<IEnumerable<ExcelFile>> GetFiles();

        public Task<BaseResponse<ExcelFile>> DeleteFile(int id);

        public Task<BaseResponse<ExcelFile>> SelectFile(int id);
    }
}
