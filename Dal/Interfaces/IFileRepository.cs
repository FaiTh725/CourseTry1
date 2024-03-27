using CourseTry1.Domain.Entity;
using Microsoft.IdentityModel.Tokens;

namespace CourseTry1.Dal.Interfaces
{
    public interface IFileRepository
    {
        public Task Add(IFormFile file, IWebHostEnvironment appEnvironment);

        public Task<ExcelFile> GetByName(string name);

        public Task<ExcelFile> GetById(int id);

        public IQueryable<ExcelFile> GetAll();

        public Task DeleteFile(ExcelFile excelFile);

        public Task<ExcelFile> UpdateFile(ExcelFile excelFile);
    }
}
