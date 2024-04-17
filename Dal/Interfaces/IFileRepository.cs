using CourseTry1.Domain.Entity;
using Microsoft.IdentityModel.Tokens;

namespace CourseTry1.Dal.Interfaces
{
    public interface IFileRepository
    {
        public string NameFileFromBNTU {  get; }

        public Task Add(IFormFile file, IWebHostEnvironment appEnvironment);

        public Task<bool> Add(string url, IWebHostEnvironment appEnvironment);

        public bool Delete(string fileName, IWebHostEnvironment appEnvironment);

        public Task<ExcelFile> GetByName(string name);

        public Task<ExcelFile> GetById(int id);

        public IQueryable<ExcelFile> GetAll();

        public Task DeleteFile(ExcelFile excelFile);

        public Task<ExcelFile> UpdateFile(ExcelFile excelFile);

        public Task UnSelectFiles();
    }
}
